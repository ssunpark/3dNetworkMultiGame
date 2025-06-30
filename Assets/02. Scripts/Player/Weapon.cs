using UnityEngine;

public class Weapon : MonoBehaviour
{
    private PlayerAttackAbility _playerAttackAbility;

    private void Start()
    {
        _playerAttackAbility = GetComponentInParent<PlayerAttackAbility>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == _playerAttackAbility.transform)
        {
            return;
        }
        
        IDamaged damagedObject = other.GetComponent<IDamaged>();
        if (damagedObject != null)
        {
            _playerAttackAbility.Hit(other);
        }
    }
    
}
