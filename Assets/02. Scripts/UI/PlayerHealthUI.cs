using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
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
        UpdateHealthUI();

    }

    public void UpdateHealthUI()
    {
        float ratio = _player.Stat.CurrentHealth / _player.Stat.MaxHealth;
        FillImage.fillAmount = ratio;
    }
}
