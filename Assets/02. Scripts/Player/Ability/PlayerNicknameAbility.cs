using TMPro;
using UnityEngine;
// 뿡
public class PlayerNicknameAbility : PlayerAbility
{
    public TextMeshProUGUI NicknameTextUI;

    private void Start()
    {
        NicknameTextUI.text = $"{_photonView.Owner.NickName}_{_photonView.Owner.ActorNumber}";

        if (_photonView.IsMine)
        {
            NicknameTextUI.color = Color.magenta;
        }
        else
        {
            NicknameTextUI.color = Color.yellow;
        }
    }
}
