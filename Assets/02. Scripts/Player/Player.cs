using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour, IDamaged
{
    public PlayerStat Stat;
    public PlayerState State;

    private readonly Dictionary<Type, PlayerAbility> _abilitiesCache = new Dictionary<Type, PlayerAbility>();

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

        GameObject minimapCamObj = GameObject.FindWithTag("MinimapCamera");
        if (minimapCamObj != null)
        {
            CopyPosition copyPosition = minimapCamObj.GetComponent<CopyPosition>();
            if (copyPosition != null)
            {
                copyPosition.SetTarget(transform);
            }
        }

        if (_photonView.IsMine)
        {
            GameObject helathBar = GameObject.FindGameObjectWithTag("HealthBar");
            if (helathBar != null)
            {
                PlayerHealthUI playerHealthUI = helathBar.GetComponent<PlayerHealthUI>();
                if (playerHealthUI != null)
                {
                    playerHealthUI.SetPlayer(this);
                    playerHealthUI.UpdateHealthUI();
                }
            }

            GameObject staminaBar = GameObject.FindGameObjectWithTag("StaminaBar");
            if (staminaBar != null)
            {
                PlayerStaminaUI playerStaminaUI = staminaBar.GetComponent<PlayerStaminaUI>();
                if (playerStaminaUI != null)
                {
                    playerStaminaUI.SetPlayer(this);
                    playerStaminaUI.UpdateStaminaUI();
                }
            }

            PlayerHealthBarAbility playerHealthBarAbility = GetComponentInChildren<PlayerHealthBarAbility>();
            if (playerHealthBarAbility != null)
            {
                playerHealthBarAbility.Refresh();
            }
        }
        
        GameObject Bear =  GameObject.FindGameObjectWithTag("Bear");
        if (Bear != null)
        {
            BearFSM bearFSM = Bear.GetComponent<BearFSM>();
            if (bearFSM != null)
            {
                bearFSM.SetTarget(transform);
            }
        }
    }

    [PunRPC]
    public void Damaged(float damage, int actorNumber)
    {
        Stat.CurrentHealth = Mathf.Max(0, Stat.CurrentHealth - damage);
        GetAbility<PlayerHealthBarAbility>().Refresh();
        // Debug.Log($"남은 체력: {Stat.CurrentHealth}");

        _photonView.RPC(nameof(PlayerHitAbility.PlayerHitAnimation), RpcTarget.All);

        if (Stat.CurrentHealth <= 0)
        {
            RoomManager.Instance.OnPlayerDeath(_photonView.Owner.ActorNumber, actorNumber);
            if (_photonView.IsMine)
            {
                _photonView.RPC(nameof(PlayerDieAbility.PlayerDieAnimation), RpcTarget.All);
                var actorPlayer = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
                _photonView.RPC(nameof(RequestAddkillCount), actorPlayer);
                MakeScoreItems(Random.Range(1, 4));
                MakeStatItems();
            }
        }
    }

    [PunRPC]
    public void RequestAddkillCount()
    {
        ScoreManager.Instance.AddKillCount();
    }

    private void MakeScoreItems(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            // 포톤의 네트워크 객체의 생명 주기
            // Player : 플레이어가 생성하고, 플레이어가 나가면 자동 삭제(PhotonNetwork.Instantiate/Destroy)
            // Room : 룸이 생성하고, 룸이 없어지면 삭제.. (PhotonNetwork.InstantiateRoomObject/Destroy)
            ItemObjectFactory.Instance.RequestCreate(EItemType.Score, transform.position + new Vector3(0, 2, 0));
        }
    }

    private void MakeStatItems()
    {
        if (Random.value < 0.3f)
        {
            ItemObjectFactory.Instance.RequestCreate(EItemType.Stamina, transform.position + new Vector3(0, 0.5f, 0));
        }
        if (Random.value < 0.2)
        {
            ItemObjectFactory.Instance.RequestCreate(EItemType.Health, transform.position + new Vector3(0, 0.5f, 0));
        }
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
        Type type = typeof(T);

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
        if (CurrentState == newState)
        {
            return;
        }

        CurrentState = newState;
    }
}