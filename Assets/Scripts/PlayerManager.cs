using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public enum PlayerID {
        P1,
        P2
    }

    public PlayerInfo GetPlayerInfo(PlayerID id) => _playerInfos.First(p => p.ID == id);
    
    public List<PlayerDisplay> PlayerDisplays;
    private List<PlayerInfo> _playerInfos;

    [Header("Game Data")] public GameData GameData;

    private void Start() {
        
        // Create Players
        foreach (PlayerDisplay display in PlayerDisplays) {
            display.Initialize(CreatePlayer(), GameData);
        }
        
    }
    

    private PlayerInfo CreatePlayer () {
        PlayerInfo newPlayer = new PlayerInfo();
        newPlayer.HealthMax = newPlayer.HealthCurrent = GameData.PlayerStartingHealth;
        newPlayer.Currencies = new Dictionary<string, PlayerCurrency>();
        foreach (CurrencyData currency in GameData.Currencies) {
            newPlayer.Currencies.Add(currency.ID, new PlayerCurrency(currency));
        }
        _playerInfos.Add(newPlayer);
        return newPlayer;
    }
    
}