using System;
using Photon.Pun;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            PhotonView pv = player.GetComponent<PhotonView>();
            if (pv.IsMine)
            {
                pv.RPC(nameof(PlayerDieAbility.PlayerDieAnimation), RpcTarget.All);
            }
        }
    }
}
