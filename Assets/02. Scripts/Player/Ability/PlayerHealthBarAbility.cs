using Photon.Pun;
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
        var ratio = _owner.Stat.CurrentHealth / _owner.Stat.MaxHealth;
        FillImage.fillAmount = ratio;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_owner.Stat.CurrentHealth / _owner.Stat.MaxHealth);
        }
        else if (stream.IsReading)
        {
            var value = (float)stream.ReceiveNext();
            FillImage.fillAmount = value;
        }
    }
}