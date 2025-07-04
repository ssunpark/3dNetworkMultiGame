using UnityEngine;

public class Weapon : MonoBehaviour
{
    private PlayerAttackAbility _playerAttackAbility;

    private void Start()
    {
        _playerAttackAbility = GetComponentInParent<PlayerAttackAbility>();

        ScoreManager.Instance.OnDataChanged += Refresh;
        Refresh();
    }

    private void Refresh()
    {
        int score = ScoreManager.Instance.Score;
        int factor = 1 + score / 10000;
        transform.localScale = new Vector3(factor, factor, factor);
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
