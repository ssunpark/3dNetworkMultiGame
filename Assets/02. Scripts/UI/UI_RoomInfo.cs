using TMPro;
using UnityEngine;

// 뿡
public class UI_RoomInfo : MonoBehaviour
{
    public TextMeshProUGUI NameTextUI;
    public TextMeshProUGUI CountTextUI;

    private void Start()
    {
        RoomManager.Instance.OnRoomDataChanged += Refresh;
        Refresh();
    }

    private void Refresh()
    {
        var room = RoomManager.Instance.Room;
        if (room == null) return;

        NameTextUI.text = room.Name;
        CountTextUI.text = $"{room.PlayerCount}/{room.MaxPlayers}";
    }

    public void OnClickExitButton()
    {
        Exit();
    }

    private void Exit()
    {
        // Todo: 언젠가 구현할 것이다..
    }
}