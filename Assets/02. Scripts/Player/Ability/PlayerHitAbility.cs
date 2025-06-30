using Photon.Pun;
using UnityEngine;

public class PlayerHitAbility : PlayerAbility
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    [PunRPC]
    public void PlayerHitAnimation()
    {
        _animator.SetTrigger("GetHit");
        if (_photonView.IsMine)
        {
            CameraShaker.Instance.Shake();
        }
    }
}
