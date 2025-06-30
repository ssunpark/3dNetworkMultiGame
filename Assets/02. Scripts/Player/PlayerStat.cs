using System;
using UnityEngine;

[Serializable]
public class PlayerStat
{
    [Header("물리 변수")]
    public float Movespeed = 5f;
    public float RunSpeed = 10f;
    public float RotationSpeed = 2.5f;
    public float JumpPower = 2.5f;
    public float RotationPower = 100f;
    public float AttackSpeed = 1.2f;    // 초당 1.2번 공격할 수 있다.

    [Header("스테미너 변수")]
    public float MaxStamina = 100f;
    public float CurrentStamina;
    public float RunStamina = 10f;
    public float AttackStamina = 20f;
    public float JumpStamina = 10f;
    public float StaminRecovery = 20f;
}