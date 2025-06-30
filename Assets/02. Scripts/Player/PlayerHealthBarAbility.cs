using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarAbility : PlayerAbility
{
    public Image FillImage;
    
    private void Update()
    {
        if (_owner == null) return;
        Refresh();
    }

    public void Refresh()
    {
        float ratio = _owner.Stat.CurrentHealth / _owner.Stat.MaxHealth;
        FillImage.fillAmount = ratio;
    }
}
