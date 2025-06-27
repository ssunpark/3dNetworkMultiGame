using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using System;
using NUnit.Framework.Constraints;

public class Player : MonoBehaviour
{
    public PlayerStat Stat;
    public PlayerState State;
    
    public PlayerState CurrentState { get; private set; } = PlayerState.Idle;

    private Dictionary<Type, PlayerAbility> _abilitiesCache = new();

    private void Awake()
    {
        // 모든 어빌리티를 찾아서 캐싱한다.
        /*PlayerAbility[] abilities = GetComponents<PlayerAbility>();
        foreach (PlayerAbility ability in abilities)
        {
            _abilitiesCache[ability.GetType()] = ability;
        }*/
    }

    private void Start()
    {
        GameObject minimapCamObj = GameObject.FindWithTag("MinimapCamera");
        if (minimapCamObj != null)
        {
            CopyPosition copyPosition = minimapCamObj.GetComponent<CopyPosition>();
            if (copyPosition != null)
            {
                copyPosition.SetTarget(transform);
            }
        }
    }

    public T GetAbility<T>() where T : PlayerAbility
    {
        var type = typeof(T);

        if (_abilitiesCache.TryGetValue(type, out PlayerAbility ability))
        {
            return ability as T;
        }

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