using UnityEngine;

public class PlayerStaminaAbility : PlayerAbility
{
    private void Start()
    {
        _owner.Stat.CurrentStamina = _owner.Stat.MaxStamina;
    }
    private void Update()
    {
        switch (_owner.CurrentState)
        {
            case PlayerState.Idle:
                RecoverStamina();
                break;
            case PlayerState.Walk:
                RecoverStamina();
                break;
            case PlayerState.Run:
                ConsumeStamina(_owner.Stat.RunStamina);
                break;
            case PlayerState.Jump:
                ConsumeStamina(_owner.Stat.JumpStamina);
                break;
            case PlayerState.Attack:
                ConsumeStamina(_owner.Stat.AttackStamina);
                break;
        }
    }

    private void RecoverStamina()
    {
        if (_owner.Stat.CurrentStamina < _owner.Stat.MaxStamina)
        {
            _owner.Stat.CurrentStamina += _owner.Stat.StaminRecovery * Time.deltaTime;
            _owner.Stat.CurrentStamina = Mathf.Min(_owner.Stat.CurrentStamina, _owner.Stat.MaxStamina);
        }
    }

    private void ConsumeStamina(float amount)
    {
        _owner.Stat.CurrentStamina -= amount *Time.deltaTime;
        _owner.Stat.CurrentStamina = Mathf.Max(_owner.Stat.CurrentStamina, 0f);
    }
    
    
}