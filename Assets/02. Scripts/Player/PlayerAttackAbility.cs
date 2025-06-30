using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerAttackAbility : PlayerAbility
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
        if (_photonView.IsMine == false)
        {
            return;
        }
        
        // 문제
        // ability에서 다른 ability에 접근하는 효율적인 방법
        // playermoveability의 CanMove 속성에 따라 공격 여부를 정하고 싶다.
        // bool isMove = _owner.GetAbility<PlayerMoveAbility>().IsMove;
        _time += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && (_time >= CoolTime) && (_owner.Stat.CurrentStamina >= _owner.Stat.AttackStamina))
        {
            _owner.SetState(PlayerState.Attack);
            
            // AttackAnimationRandom(); 1. 일반 메서드 호출 방식
            _photonView.RPC(nameof(AttackAnimationRandom), RpcTarget.All); // 2. RPC 메서드 호출 방식
            _time = 0f;
        }
    }

    [PunRPC]
    private void AttackAnimationRandom()
    {
        int randomIndex = Random.Range(0, _attackStates.Length);
        string triggerName = _attackStates[randomIndex];
        _animator.SetTrigger(triggerName);
    }
}
