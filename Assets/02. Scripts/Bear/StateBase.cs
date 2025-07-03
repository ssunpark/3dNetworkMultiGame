using UnityEngine;

public abstract class StateBase
{
    protected BearFSM fsm;
    
    public void SetFSM(BearFSM fsm)
    {
        this.fsm = fsm;
    }

    public virtual void OnEnter()
    {
        
    }
    public virtual void OnUpdate()
    {
        
    }
    public virtual void OnExit()
    {
        
    }
}
