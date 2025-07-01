using TMPro;
using UnityEngine;

// 뿡
public class UI_RoomLog : MonoBehaviour
{
    public TextMeshProUGUI LogTextUI;

    private string _logMessage = "방에 입장했습니다.";

    private void Start()
    {
        RoomManager.Instance.OnPlayerEntered += PlayerEnterLog;
        RoomManager.Instance.OnPlayerExited += PlayerExitLog;
        RoomManager.Instance.OnPlayerDeathed += PlayerDeathLog;

        Refresh();
    }

    private void Refresh()
    {
        LogTextUI.text = _logMessage;
    }

    public void PlayerEnterLog(string playerName)
    {
        _logMessage += $"\n<color=#FF1493>{playerName}</color>님이 <color=green>입장</color>했습니다.";
        Refresh();
    }

    public void PlayerExitLog(string playerName)
    {
        _logMessage += $"\n<color=#FF1493>{playerName}</color>님이 <color=#FFFF00>퇴장</color>했습니다.";
        Refresh();
    }

    public void PlayerDeathLog(string playerName, string attackerName)
    {
        _logMessage +=
            $"\n<color=#FF1493>{attackerName}</color>님이 <color=#FFA5000>{playerName}</color>님을 <color=red>처치</color>했습니다.";
        Refresh();
    }
}