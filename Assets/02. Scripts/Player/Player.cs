using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviour, IDamaged
{
    public PlayerStat Stat;
    public PlayerState State;

    private readonly Dictionary<Type, PlayerAbility> _abilitiesCache = new();

    private Animator _animator;

    private PhotonView _photonView;

    public PlayerState CurrentState { get; private set; } = PlayerState.Idle;
    public bool IsInputBlocked { get; private set; }

    private void Awake()
    {
        // 모든 어빌리티를 찾아서 캐싱한다.
        /*PlayerAbility[] abilities = GetComponents<PlayerAbility>();
        foreach (PlayerAbility ability in abilities)
        {
            _abilitiesCache[ability.GetType()] = ability;
        }*/
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();

        var minimapCamObj = GameObject.FindWithTag("MinimapCamera");
        if (minimapCamObj != null)
        {
            var copyPosition = minimapCamObj.GetComponent<CopyPosition>();
            if (copyPosition != null) copyPosition.SetTarget(transform);
        }

        if (_photonView.IsMine)
        {
            var helathBar = GameObject.FindGameObjectWithTag("HealthBar");
            if (helathBar != null)
            {
                var playerHealthUI = helathBar.GetComponent<PlayerHealthUI>();
                if (playerHealthUI != null)
                {
                    playerHealthUI.SetPlayer(this);
                    playerHealthUI.UpdateHealthUI();
                }
            }

            var staminaBar = GameObject.FindGameObjectWithTag("StaminaBar");
            if (staminaBar != null)
            {
                var playerStaminaUI = staminaBar.GetComponent<PlayerStaminaUI>();
                if (playerStaminaUI != null)
                {
                    playerStaminaUI.SetPlayer(this);
                    playerStaminaUI.UpdateStaminaUI();
                }
            }

            var playerHealthBarAbility = GetComponentInChildren<PlayerHealthBarAbility>();
            if (playerHealthBarAbility != null) playerHealthBarAbility.Refresh();
        }
    }

    [PunRPC]
    public void Damaged(float damage)
    {
        Stat.CurrentHealth = Mathf.Max(0, Stat.CurrentHealth - damage);
        GetAbility<PlayerHealthBarAbility>().Refresh();
        // Debug.Log($"남은 체력: {Stat.CurrentHealth}");

        _photonView.RPC(nameof(PlayerHitAbility.PlayerHitAnimation), RpcTarget.All);

        if (Stat.CurrentHealth <= 0) _photonView.RPC(nameof(PlayerDieAbility.PlayerDieAnimation), RpcTarget.All);
    }

    public void BlockInput()
    {
        IsInputBlocked = true;
    }

    public void UnblockInput()
    {
        IsInputBlocked = false;
    }

    public T GetAbility<T>() where T : PlayerAbility
    {
        var type = typeof(T);

        if (_abilitiesCache.TryGetValue(type, out var ability)) return ability as T;

        // 게으른 초기화/로딩 -> 처음에 곧바로 초기화/로딩을 하는게 아니라
        //                    필요할때만 하는.. 뒤로 미루는 기법
        ability = GetComponent<T>();

        if (ability != null)
        {
            _abilitiesCache[ability.GetType()] = ability;

            return ability as T;
        }

        throw new Exception($"어빌리티 {type.Name}을 {gameObject.name}에서 찾을 수 없습니다.");
    }

    public void SetState(PlayerState newState)
    {
        if (CurrentState == newState) return;
        CurrentState = newState;
    }
}