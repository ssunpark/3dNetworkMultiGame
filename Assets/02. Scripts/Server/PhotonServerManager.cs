using System.Collections.Generic;
using UnityEngine;

// Photon API 네임 스페이스
using Photon.Pun;
using Photon.Realtime;
using Player = Photon.Realtime.Player;

// 역할: 포톤 서버 관리자(서버 연결, 로비 입장, 방 입장, 게임 입장)
public class PhotonServerManager : MonoBehaviourPunCallbacks
{
    // MonoBehaviourPunCallbacks: 유니티 이벤트 말고도 PUN 서버 이벤트를 받을 수 있다.
    private readonly string _gameVersion = "1.0.0";
    private string _nickname = "Hesther";
    
    private void Start()
    {
        // 설정
        // 0. 데이터 송수신 빈도를 매 초당 60회로 설정한다. (기본은 10)
        PhotonNetwork.SendRate = 60; // 선호하는 값이지 보장은 안 함.
        PhotonNetwork.SerializationRate = 60;
        
        
        // 1. 버전 : 버전이 다르면 다른 서버로 접속이 된다.
        PhotonNetwork.GameVersion = "0.0.1";
        // 2. 닉네임 : 게임에서 사용할 사용자의 별명(중복 가능 -> 판별을 위해서는 ActorID)
        PhotonNetwork.NickName = "Hesther";
        
        // 방장이 로드한 씬으로 다른 참여자가 똑같이 이동하게끔 동기화해주는 옵션
        // 방장: 방을 만든 소유자이자 " 마스터 클라이언트" (방마다 한명의 마스터 클라이언트가 존재)
        PhotonNetwork.AutomaticallySyncScene = true;
        
        // 설정 값들을 이용해 서버 접속 시도
        // 네임 서버 접속 -> 방 목록이 있는 마스터 서버까지 접속이 됩니다.
        PhotonNetwork.ConnectUsingSettings();
    }

    // 포톤 서버에 접속 후 호출되는 콜백(이벤트) 함수
    public override void OnConnected()
    {
        Debug.Log("네임 서버 접속 완료");
        Debug.Log(PhotonNetwork.CloudRegion);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버 접속 완료");
        Debug.Log($"InLobby: {PhotonNetwork.InLobby}");

        // 디폴드 로비(채널)입장
        PhotonNetwork.JoinLobby();
        // PhotonNetwork.JoinLobby(TypedLobby.Default);
    }
    
    // 로비에 접속하면 호출되는 함수
    public override void OnJoinedLobby()
    {
        Debug.Log("로비(채널) 입장 완료!");
        Debug.Log($"InLobby: {PhotonNetwork.InLobby}"); // 로비 입장 유무
        
        // 랜덤 방에 들어간다.
        PhotonNetwork.JoinRandomRoom();
    }
    
    // 방에 입장한 후 호출되는 함수
    public override void OnJoinedRoom()
    {
        Debug.Log($"룸 입장: {PhotonNetwork.InRoom} : {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log($"플레이어: {PhotonNetwork.CurrentRoom.PlayerCount}명");
        
        // 룸에 접속한 사용자 정보
        Dictionary<int, Photon.Realtime.Player> roomPlayers = PhotonNetwork.CurrentRoom.Players;
        foreach (KeyValuePair<int, Photon.Realtime.Player> player in roomPlayers)
        {
            Debug.Log($"{player.Value.NickName} : {player.Value.ActorNumber}");
            // ActorNumber는 Room 안에서의 플레이어에 대한 판별 ID. 들어온 순서대로 1-2-3 부여
            Debug.Log(player.Value.UserId); // 이게 진짜 고유 ID. 친구 기능, 귓속말 등등에 쓰인다.
        }
        
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }
    
    // 랜덤 방 입장에 실패하면 호출되는 함수
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"랜덤 방 입장에 실패했습니다: {returnCode}, {message}");
        
        // 룸 속성 정의
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        roomOptions.IsOpen = true; // 룸 입장 가능 여부
        roomOptions.IsVisible = true; // 로비(채널) 룸 목록에 노출시킬지 여부
        
        // Room 생성
        PhotonNetwork.CreateRoom("test", roomOptions);
    }

    // 룸 생성에 실패했을 때 호출되는 함수
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"룸 생성에 실패했습니다: {returnCode}, {message}");
    }
    
    // 룸 생성에 성공했을 때 호출되는 함수
    public override void OnCreatedRoom()
    {
        Debug.Log($"룸 생성에 성공했습니다: {PhotonNetwork.CurrentRoom.Name}");
    }
}
