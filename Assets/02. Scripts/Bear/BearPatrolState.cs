using UnityEngine;
using UnityEngine.AI;
public class BearPatrolState : StateBase
{
    private float _timer = 0f;
    private Vector3 _destination;
    
    public override void OnEnter()
    {
        _timer = 0f;
        SetRandomDestination(); // 목적지 설정
        fsm.Animator.SetTrigger("Patrol");
        fsm.Agent.isStopped = false; // 에이전트
        fsm.Agent.SetDestination(_destination); // 실제로 가라고 명령
    }

    public override void OnUpdate()
    {
        _timer += Time.deltaTime;
        
        // 타겟이 TraceRange 내로 진입하면 바로 Trace 모드로 진입
        if (fsm.Target != null)
        {
            float distance = Vector3.Distance(fsm.transform.position, fsm.Target.transform.position);
            if (distance <= fsm.TraceRange)
            {
                fsm.TransitionTo(fsm.BearTrace);
                return;
            }
        }
        
        // 목적지 도착
        if (!fsm.Agent.pathPending && fsm.Agent.remainingDistance <= fsm.Agent.stoppingDistance)
        {
            fsm.TransitionTo(fsm.BearIdle);
            return;
        }
        
        //  PatrolTime 지나면 Idle 상태로 진입
        if (_timer >= fsm.PatrolTime)
        {
            fsm.TransitionTo(fsm.BearIdle);
            _timer = 0f;
            return;
        }
    }
    
    public override void OnExit()
    {
        fsm.Agent.isStopped = true;
    }

    private void SetRandomDestination()
    {
        Vector3 randomDirection =  Random.insideUnitSphere * 5f;
        randomDirection.y = 0;
        
        Vector3 rawDestination = fsm.transform.position + randomDirection;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, 5f, NavMesh.AllAreas))
        {
            _destination = hit.position;
        }
        else
        {
            _destination = fsm.transform.position;
        }
        
        Debug.DrawLine(fsm.transform.position, _destination, Color.red, 5f);
        // _destination = fsm.transform.position + randomDirection;
    }
}
