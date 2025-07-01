using Photon.Pun;
using UnityEngine;

public class PlayerHitAbility : PlayerAbility
{
    public GameObject EffectParticlePrefab;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    [PunRPC]
    public void PlayerHitAnimation()
    {
        _animator.SetTrigger("GetHit");
        if (_photonView.IsMine) CameraShaker.Instance.Shake();
        var effectPosition = transform.position + new Vector3(0, 0.3f, 0);
        Instantiate(EffectParticlePrefab, effectPosition, Quaternion.identity);
    }
}