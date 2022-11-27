using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public enum PlayerID {
        P1,
        P2
    }

    public static PlayerID OpponentOf(PlayerID id) {
        switch (id) {
            case PlayerID.P1:
                return PlayerID.P2;
            case PlayerID.P2:
                return PlayerID.P1;
            default:
                throw new ArgumentOutOfRangeException(nameof(id), id, null);
        }
    }

    public PlayerInfo GetPlayerInfo(PlayerID id) =>_playerInfos.First(p => p.ID == id);
    
    public List<PlayerDisplay> PlayerDisplays;
    private List<PlayerInfo> _playerInfos = new List<PlayerInfo>();

    [Header("Game Data")] public GameData GameData;

    public PlayerManagerEffectActions EffectActions;
    private PlayerManagerUpdateLoop _updateLoop;

    public event Action OnPlayersCreated;
    [HideInInspector]
    public bool PlayersCreated;
    
    private void Start() {
        // Create Players
        PlayerInfo player = CreatePlayer(0);
        PlayerDisplays[0].Initialize(player, GameData);
        player = CreatePlayer(1);
        PlayerDisplays[1].Initialize(player, GameData);

        EffectActions = new PlayerManagerEffectActions(this);
        _updateLoop = new PlayerManagerUpdateLoop(this);

        PlayersCreated = true;
        OnPlayersCreated?.Invoke();
    }
    

    private PlayerInfo CreatePlayer(int playerIndex) {
        PlayerInfo newPlayer = new PlayerInfo();
        newPlayer.ID = (PlayerID)playerIndex;
        newPlayer.HealthMax = newPlayer.HealthCurrent = GameData.PlayerStartingHealth;
        
        // Currencies
        newPlayer.Currencies = new Dictionary<string, PlayerCurrency>();
        foreach (CurrencyData currency in GameData.Currencies) {
            newPlayer.Currencies.Add(currency.ID, new PlayerCurrency(currency));
        }
        
        // Abilities
        newPlayer.Abilities = new List<Ability>();
        foreach (AbilityData ability in GameData.Abilities) {
            newPlayer.Abilities.Add(new Ability(ability, (PlayerID)playerIndex));
        }
        
        _playerInfos.Add(newPlayer);
        return newPlayer;
    }

    private void Update() {
        _updateLoop.DoUpdate();
    }
    
}