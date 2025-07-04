using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
public class LobbyScene : MonoBehaviour
{
    public TMP_InputField NicknameInputField;
    public TMP_InputField RoomNameInputField;

    // 방 만들기 함수를 호출
    public void OnClickMakeRoomButton()
    {
        MakeRoom();
    }

    private void MakeRoom()
    {
        string nickname = NicknameInputField.text;
        string roomName = RoomNameInputField.text;

        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(roomName))
        {
            return;
        }
        
        PhotonNetwork.NickName = nickname;
        
        // 룸 속성 정의
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        roomOptions.IsOpen = true; // 룸 입장 가능 여부
        roomOptions.IsVisible = true; // 로비(채널) 룸 목록에 노출시킬지 여부
        
        // Room 생성
        // PhotonNetwork.CreateRoom(roomName, roomOptions);
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }
    
}
