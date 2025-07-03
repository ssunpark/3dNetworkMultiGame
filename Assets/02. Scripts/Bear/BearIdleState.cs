using UnityEngine;
public class BearIdleState : StateBase
{
    private float _timer = 0f;
    
    public override void OnEnter()
    {
        _timer = 0f;
        fsm.Animator.SetTrigger("Idle");
        fsm.Agent.isStopped = true;
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
        
        // idleTimer 시간을 경과하면 Patrol 모드로 진입
        if (_timer >= fsm.PatrolTime)
        {
            fsm.TransitionTo(fsm.BearPatrol);
            _timer = 0f;
            return;
        }
        
    }
    
    public override void OnExit()
    {
        
    }

}
