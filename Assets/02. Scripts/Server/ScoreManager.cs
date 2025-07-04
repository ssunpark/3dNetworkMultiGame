using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public class ScoreManager : MonoBehaviourPunCallbacks
{
    private int _killCount = 0;
    
    public static ScoreManager Instance
    {
        get; private set;
    }

    private void Awake()
    {
        Instance = this;
    }
    
    private Dictionary<string, int> _scores = new Dictionary<string, int>();
    public Dictionary<string, int> Scores => _scores;
    
    public event Action OnDataChanged;

    private int _scoreAmount = 0;
    public int Score => _scoreAmount;
    
    public void Start()
    {
        Refresh();
    }


    public void Refresh()
    {
        Hashtable hashTable = new Hashtable();
        hashTable.Add("Score", _killCount*5000+_scoreAmount); //_score
        
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashTable);
    }

    [PunRPC]
    public void AddKillCount()
    {
        _killCount += 1;
        Refresh();
    }
    
    public void AddScore(int addScore)
    {
        _scoreAmount += addScore;
        Refresh();
    }

    // 플레이어의 커스텀 프로퍼티가 변경되면 호출되는 콜백 함수
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable hashtable)
    {
        _scores.Clear();
        //Debug.Log($"Player {targetPlayer.NickName}_{targetPlayer.ActorNumber}의 점수: {hashtable["Score"]}");

        var roomPlayers = PhotonNetwork.PlayerList;
        foreach (Photon.Realtime.Player player in roomPlayers)
        {
            if (player.CustomProperties.ContainsKey("Score"))
            {
                _scores[$"{player.NickName}_{player.ActorNumber}"] = (int)player.CustomProperties["Score"];
            }
        }

        OnDataChanged?.Invoke();
    }

}
