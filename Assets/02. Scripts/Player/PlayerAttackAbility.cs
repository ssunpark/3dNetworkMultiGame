using UnityEngine;
// 뿡
public class PlayerAttackAbility : MonoBehaviour
{
    public float CoolTime = 0.6f;
    private float _time = 0f;
    
    private Animator _animator;
    private string[] _attackStates = {"Attack1", "Attack2", "Attack3"};

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _time = 0f;
    }

    private void Update()
    {
        _time += Time.deltaTime;
        Debug.Log(_time);
        if (Input.GetMouseButtonDown(0) && _time >= CoolTime)
        {
            AnimationRandom();
            _time = 0f;
            Debug.Log("타임 초기화!!!");
        }
    }

    private void AnimationRandom()
    {
        int randomIndex = Random.Range(0, _attackStates.Length);
        string triggerName = _attackStates[randomIndex];
        _animator.SetTrigger(triggerName);
    }
}
