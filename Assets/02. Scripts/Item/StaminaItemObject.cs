using Photon.Pun;
using UnityEngine;
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class StaminaItemObject : MonoBehaviourPun
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Stat.CurrentStamina += 10;
                Debug.Log($"{player.Stat.CurrentStamina}");

                ItemObjectFactory.Instance.RequestDelete(photonView.ViewID);
            }
        }
    }
}