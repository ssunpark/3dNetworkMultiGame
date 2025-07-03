using UnityEngine;
public class BearDieState : StateBase
{
    private float _timer = 0f;
    
    public override void OnEnter()
    {
        _timer = 0f;
        fsm.Animator.SetTrigger("Die");
        fsm.CurrentHealth = 0f;
        fsm.Agent.isStopped = true;

        Collider collider = fsm.GetComponent<Collider>();
        if(collider != null) collider.enabled = false;
        
        Debug.Log("곰 사망");
    }

    public override void OnUpdate()
    {
        _timer += Time.deltaTime;
        if (_timer >= fsm.RespawnTime)
        {
            // 리스폰
            fsm.TransitionTo(fsm.BearIdle);
            _timer = 0f;
        }
        
    }
    
    public override void OnExit()
    {
        _timer = 0f;
        if (fsm != null)
        {
            fsm.Respawn();
        }
    }
}
