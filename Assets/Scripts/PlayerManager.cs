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
    private List<PlayerInfo> _playerInfos = new List<PlayerInfo>();

    public PlayerManagerEffectActions EffectActions;

    [Header("Game Data")] public GameData GameData;

    private void Start() {
        // Create Players
        PlayerInfo player = CreatePlayer(0);
        PlayerDisplays[0].Initialize(player, GameData);
        player = CreatePlayer(1);
        PlayerDisplays[0].Initialize(player, GameData);

        EffectActions = new PlayerManagerEffectActions(this);
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
    
}