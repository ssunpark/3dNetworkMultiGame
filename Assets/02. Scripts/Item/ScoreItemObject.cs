using Photon.Pun;
using UnityEngine;
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ScoreItemObject : MonoBehaviourPun
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                ScoreManager.Instance.AddScore(10);
                Debug.Log($"{ScoreManager.Instance.Score}");

                ItemObjectFactory.Instance.RequestDelete(photonView.ViewID);
            }
        }
    }
}