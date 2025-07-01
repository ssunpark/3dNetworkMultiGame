using Photon.Pun;
using UnityEngine;

public class PlayerAttackAbility : PlayerAbility
{
    public float CoolTime = 0.6f;

    public Collider WeaponCollider;
    private readonly string[] _attackStates = { "Attack1", "Attack2", "Attack3" };

    private Animator _animator;
    private float _time;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _time = 0f;
        DeActiveCollider();
    }

    private void Update()
    {
        if (_photonView.IsMine == false || _owner.IsInputBlocked) return;

        // 문제
        // ability에서 다른 ability에 접근하는 효율적인 방법
        // playermoveability의 CanMove 속성에 따라 공격 여부를 정하고 싶다.
        // bool isMove = _owner.GetAbility<PlayerMoveAbility>().IsMove;
        _time += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && _time >= CoolTime && _owner.Stat.CurrentStamina >= _owner.Stat.AttackStamina)
        {
            _owner.SetState(PlayerState.Attack);

            // AttackAnimationRandom(); 1. 일반 메서드 호출 방식
            _photonView.RPC(nameof(AttackAnimationRandom), RpcTarget.All); // 2. RPC 메서드 호출 방식
            _time = 0f;
        }
    }

    public void ActiveCollider()
    {
        WeaponCollider.enabled = true;
    }

    public void DeActiveCollider()
    {
        WeaponCollider.enabled = false;
    }

    [PunRPC]
    private void AttackAnimationRandom()
    {
        var randomIndex = Random.Range(0, _attackStates.Length);
        var triggerName = _attackStates[randomIndex];
        _animator.SetTrigger(triggerName);
    }

    public void Hit(Collider other)
    {
        if (_photonView.IsMine == false) return;

        DeActiveCollider();

        // 데미지를 받는 오브젝트의 데미지 처리
        if (other.GetComponent<IDamaged>() == null) return;
        var otherPhotonView = other.GetComponent<PhotonView>();
        otherPhotonView.RPC(nameof(Player.Damaged), RpcTarget.AllBuffered, _owner.Stat.Damage,
            _photonView.Owner.ActorNumber);
        // otherPhotonView.RPC(nameof(Player.Damaged), RpcTarget.AllBuffered, _owner.Stat.Damage);
    }
}