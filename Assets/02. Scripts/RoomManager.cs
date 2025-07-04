using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public Room Room { get; private set; }

    public static RoomManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public event Action OnRoomDataChanged;
    public event Action<string> OnPlayerEntered;
    public event Action<string> OnPlayerExited;

    bool _initialized = false;
    
    public override void OnJoinedRoom()
    {
        Debug.Log("111111111");
        Init();
    }
    
    private void Start()
    {
        Debug.Log("2222");

        if (PhotonNetwork.InRoom)
        {
            Init();
        }
    }
    
    // 방에 입장하면 자동으로 호출되는 함수
    public void Init()
    {
        if (_initialized)
        {
            return;
        }
        Debug.Log("33333333333");
        _initialized = true;

        // if (PhotonNetwork.InRoom) return;
        
        // 1. 플레이어 생성
        GeneratePlayer();

        // 2. 룸 설정
        SetRoom();

        OnRoomDataChanged?.Invoke();
    }

    // 새로운 플레이어가 방에 입장하면 자동으로 호출되는 함수
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        OnRoomDataChanged?.Invoke();
        // 1. 요거는 manger가 UI에 대한 의존성이 생긴다.

        // 2. UI가 직접 서버 로직을 아는 것은 스마트UI

        // 3. 결국 관리는 Manager가...
        OnPlayerEntered?.Invoke(newPlayer.NickName + "_" + newPlayer.ActorNumber);
    }

    // 플레이어가 방에서 퇴장하면 자동으로 호출되는 함수
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        OnRoomDataChanged?.Invoke();
        OnPlayerExited?.Invoke(otherPlayer.NickName + "_" + otherPlayer.ActorNumber);
    }

    private void GeneratePlayer()
    {
        PhotonServerManager.Instance.SpawnPlayer();
    }

    private void SetRoom()
    {
        Room = PhotonNetwork.CurrentRoom;
        Debug.Log(Room.Name);
        Debug.Log(Room.PlayerCount);
        Debug.Log(Room.MaxPlayers);
    }

    public event Action<string, string> OnPlayerDeathed;

    public void OnPlayerDeath(int actorNumber, int otherActorNumber)
    {
        // actorNumber가 otherActorNumber에 의해 죽었다.
        var deathedNickname = Room.Players[actorNumber].NickName;
        var attackerNickname = Room.Players[otherActorNumber].NickName;

        OnPlayerDeathed?.Invoke(deathedNickname, attackerNickname);
    }
}