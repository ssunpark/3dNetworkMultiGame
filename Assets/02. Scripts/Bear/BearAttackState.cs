using Photon.Pun;
using UnityEngine;
public class BearAttackState : StateBase
{
    private bool _hasAttacked = false;
    
    public override void OnEnter()
    {
        _hasAttacked = false;
        fsm.Animator.SetTrigger("Attack01");
        fsm.Agent.isStopped = true;

        if (fsm.Target != null)
        {
            float distance = Vector3.Distance(fsm.transform.position, fsm.Target.position);
            if (distance > fsm.AttackRange)
            {
                IDamaged target = fsm.Target.GetComponent<IDamaged>();
                PhotonView targetPhotonView = fsm.Target.GetComponent<PhotonView>();
                if (target != null)
                {
                    target.Damaged(fsm.DamageAmount, targetPhotonView.OwnerActorNr);
                    _hasAttacked = true;
                }
            }
        }
    }

    public override void OnUpdate()
    {
        AnimatorStateInfo stateInfo = fsm.Animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Attack01") && stateInfo.normalizedTime >= 1.0f)
        {
            fsm.TransitionTo(fsm.BearTrace);
        }
    }
    
    public override void OnExit()
    {
        _hasAttacked = false;
    }
}
