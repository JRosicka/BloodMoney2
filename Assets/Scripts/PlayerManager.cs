using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager Instance;
    
    public List<PlayerDisplay> PlayerDisplays;
    private List<PlayerInfo> _playerInfos;

    [Header("Game Data")] public GameData GameData;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        
        // Create Players
        foreach (PlayerDisplay display in PlayerDisplays) {
            display.PlayerInfo = CreatePlayer();
        }
        
    }

    private PlayerInfo CreatePlayer () {
        PlayerInfo newPlayer = new PlayerInfo();
        newPlayer.HealthMax = newPlayer.HealthCurrent = GameData.PlayerStartingHealth;
        newPlayer.Currencies = new Dictionary<string, PlayerCurrency>();
        foreach (CurrencyData currency in GameData.Currencies) {
            newPlayer.Currencies.Add(currency.ID, new PlayerCurrency(currency.ID));
        }
        return newPlayer;
    }
    
}
