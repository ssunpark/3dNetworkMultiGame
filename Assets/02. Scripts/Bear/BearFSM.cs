using UnityEngine;
using UnityEngine.AI;
public class BearFSM : MonoBehaviour, IDamaged
{
    public float PatrolTime = 7f;
    public float TraceRange = 18f;
    public float AttackRange = 7f;
    public float DamageAmount = 20f;
    public float MaxHealth = 100f;
    public float CurrentHealth = 100f;
    public float RespawnTime = 30f;

    public Transform Target { get; private set; }
    public NavMeshAgent Agent {get; private set;}
    public Animator Animator {get; private set;}
    
    // currentState: 현재 실행 중인 상태
    private StateBase _currentState;
    
    // 상태 인스턴스 생성
    private BearIdleState _bearBearIdleState;
    private BearPatrolState _bearBearPatrolState;
    private BearTraceState _bearBearTraceState;
    private BearAttackState _bearBearAttackState;
    private BearDamagedState _bearBearDamagedState;
    private BearDieState _bearBearDieState;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();
    }
    
    // Start(): 처음 상태 설정
    private void Start()
    {
        MaxHealth = CurrentHealth;
        
        _bearBearIdleState = new BearIdleState();
        _bearBearIdleState.SetFSM(this);
        
        _bearBearPatrolState = new BearPatrolState();
        _bearBearPatrolState.SetFSM(this);
        
        _bearBearTraceState = new BearTraceState();
        _bearBearTraceState.SetFSM(this);
        
        _bearBearAttackState = new BearAttackState();
        _bearBearAttackState.SetFSM(this);
        
        _bearBearDamagedState = new BearDamagedState();
        _bearBearDamagedState.SetFSM(this);
        
        _bearBearDieState = new BearDieState();
        _bearBearDieState.SetFSM(this);
        
        // 초기 상태 설정
        TransitionTo(_bearBearIdleState);
    }
    // Update(): 현재 상태의 OnUpdate() 실행
    private void Update()
    {
        _currentState?.OnUpdate();
    }
    
    // 상태 전이 함수, 데미지 처리 함수 ...
    public void TransitionTo(StateBase newState)
    {
        _currentState?.OnExit();
        _currentState = newState;
        _currentState.OnEnter();
    }
    
    public void SetTarget(Transform target)
    {
        Target = target;
    }
    
    // 외부에서 사용할 수 있도록 참조 프로퍼티 추가
    public BearIdleState BearIdle => _bearBearIdleState;
    public BearPatrolState BearPatrol => _bearBearPatrolState;
    public BearTraceState BearTrace => _bearBearTraceState;
    public BearAttackState BearAttack => _bearBearAttackState;
    public BearDamagedState BearDamaged => _bearBearDamagedState;
    public BearDieState BearDie => _bearBearDieState;

    public void Damaged(float damage, int actorNumber)
    {
        if (CurrentHealth <= 0) return;
        
        CurrentHealth -= Mathf.RoundToInt(damage);
        Debug.Log($"곰이 {damage}만큼 피해를 입음. 현재 체력: {CurrentHealth}");
        TransitionTo(BearDamaged);
    }

    public void Respawn()
    {
        CurrentHealth = MaxHealth;
        
        Collider collider = GetComponent<Collider>();
        if(collider != null) collider.enabled = true;
        
        transform.position = GetRandomRespawnPosition();
        
        Debug.Log("곰 리스폰 완료");
    }

    public Vector3 GetRandomRespawnPosition()
    {
        Vector3 randomOffset = Random.insideUnitSphere * 5f;
        randomOffset.y = 0;
        return transform.position + randomOffset;
    }
}