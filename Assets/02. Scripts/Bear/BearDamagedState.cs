using UnityEngine;
public class BearDamagedState : StateBase
{
    public override void OnEnter()
    {
        fsm.Animator.SetTrigger("Damaged");
        fsm.Agent.isStopped = true;
    }

    public override void OnUpdate()
    {
        if (fsm.CurrentHealth <= 0)
        {
            fsm.TransitionTo(fsm.BearDie);
            return;
        }
        
        AnimatorStateInfo stateInfo = fsm.Animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Attack01") && stateInfo.normalizedTime >= 1.0f)
        {
            fsm.TransitionTo(fsm.BearIdle);
        }
    }
    
    public override void OnExit()
    {
        
    }
}
