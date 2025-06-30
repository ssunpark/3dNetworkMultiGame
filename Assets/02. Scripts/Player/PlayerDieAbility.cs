using System.Collections;
using Photon.Pun;
using UnityEngine;

public class PlayerDieAbility : PlayerAbility
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    [PunRPC]
    public void PlayerDieAnimation()
    {
        Debug.Log("죽음죽음죽음");
        _animator.SetTrigger("Die");

        if (_photonView.IsMine)
        {
            _owner.BlockInput();
        }

        StartCoroutine(PlayerRespawnCoroutine());
    }

    private IEnumerator PlayerRespawnCoroutine()
    {

        yield return new WaitForSeconds(5f);
        if (_photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject); // 죽은 본인 파괴
            PhotonServerManager.Instance.RespawnPlayer();
        }
    }
}
