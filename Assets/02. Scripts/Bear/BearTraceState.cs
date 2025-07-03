using UnityEngine;
public class BearTraceState : StateBase
{
    public override void OnEnter()
    {
        fsm.Animator.SetTrigger("Trace");
        fsm.Agent.isStopped = false;
    }

    public override void OnUpdate()
    {
        if (fsm.Target != null)
        {
            fsm.TransitionTo(fsm.BearPatrol);
            return;
        }
        
        fsm.Agent.SetDestination(fsm.Target.position);
        
        float distance = Vector3.Distance(fsm.transform.position, fsm.Target.position);

        if (distance <= fsm.AttackRange)
        {
            fsm.TransitionTo(fsm.BearAttack);
            return;
        }

        if (distance > fsm.TraceRange)
        {
            fsm.TransitionTo(fsm.BearPatrol);
            return;
        }
    }
    
    public override void OnExit()
    {
        fsm.Agent.isStopped = true;
    }
}
