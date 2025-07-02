using Photon.Pun;
using UnityEngine;

// 아이템 공장: 아이템 생성을 담당
[RequireComponent(typeof(PhotonView))]
public class ItemObjectFactory : MonoBehaviourPun
{

    public Transform[] ItemSpawnPoints;
    private readonly float _spawnTime = 10f;
    private PhotonView _photonView;
    private float _timer;
    private static ItemObjectFactory _instance;
    public static ItemObjectFactory Instance => _instance;

    private void Awake()
    {
        _instance = this;
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        _timer = 0f;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _spawnTime)
        {
            StoneItemSpawn();
            _timer = 0;
        }
    }

    public void RequestCreate(EItemType itemType, Vector3 dropPosition)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Create(itemType, dropPosition);
        }
        else
        {
            _photonView.RPC(nameof(Create), RpcTarget.MasterClient, itemType, dropPosition);
        }
    }

    [PunRPC]
    private void Create(EItemType itemType, Vector3 dropPosition)
    {
        PhotonNetwork.InstantiateRoomObject($"{itemType}Item", dropPosition,
            Quaternion.identity);
    }


    public void RequestDelete(int viewId)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Delete(viewId);
        }
        else
        {
            _photonView.RPC(nameof(Delete), RpcTarget.MasterClient, viewId);
        }
    }

    [PunRPC]
    private void Delete(int viewId)
    {
        GameObject objectToDelete = PhotonView.Find(viewId)?.gameObject;
        if (objectToDelete == null)
        {
            return;
        }

        PhotonNetwork.Destroy(objectToDelete);
    }

    private void StoneItemSpawn()
    {
        int r = Random.Range(0, ItemSpawnPoints.Length);
        PhotonNetwork.Instantiate("ScoreItem", ItemSpawnPoints[r].position, ItemSpawnPoints[r].rotation);
    }
}