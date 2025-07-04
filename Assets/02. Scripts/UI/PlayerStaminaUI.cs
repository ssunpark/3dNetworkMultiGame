using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaUI : MonoBehaviour
{
    public Image FillImage;
    private Player _player;
    
    public void SetPlayer(Player player)
    {
        _player = player;
    }

    private void Update()
    {
        if (_player == null) return;
        UpdateStaminaUI();

    }

    public void UpdateStaminaUI()
    {
        float ratio = _player.Stat.CurrentStamina / _player.Stat.MaxStamina;
        FillImage.fillAmount = ratio;
    }
    
    
}
