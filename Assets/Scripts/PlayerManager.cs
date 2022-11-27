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

    public PlayerManagerEffectActions EffectActions;
    private PlayerManagerUpdateLoop _updateLoop;

    [Header("Game Data")] public GameData GameData;

    private void Start() {
        // Create Players
        int index = 0;
        foreach (PlayerDisplay display in PlayerDisplays) {
            display.Initialize(CreatePlayer(index), GameData);
            index++;
        }

        EffectActions = new PlayerManagerEffectActions(this);
        _updateLoop = new PlayerManagerUpdateLoop(this);
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
            newPlayer.Abilities.Add(new Ability(ability));
        }
        
        _playerInfos.Add(newPlayer);
        return newPlayer;
    }

    private void Update() {
        _updateLoop.DoUpdate();
    }
    
}