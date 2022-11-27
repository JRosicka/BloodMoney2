using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerUpdateLoop {

    private PlayerManager _manager;
    
    public PlayerManagerUpdateLoop(PlayerManager manager) {
        _manager = manager;
    }
    
    public void DoUpdate() {
        UpdatePlayer(_manager.GetPlayerInfo(PlayerManager.PlayerID.P1));
        UpdatePlayer(_manager.GetPlayerInfo(PlayerManager.PlayerID.P2));
    }

    private void UpdatePlayer(PlayerInfo player) {
        // Update Health
        player.HealthCurrent = Mathf.Clamp(
            player.HealthCurrent += player.HealthDelta * Time.deltaTime, 
            0f, 
            player.HealthMax);
        
        // Check for game over
        if (player.HealthCurrent <= 0f) {
            GameManager.Instance.GameOver(PlayerManager.OpponentOf(player.ID));
        }
        
        // Update Currencies
        foreach (string currencyID in player.Currencies.Keys) {
            player.Currencies[currencyID].Amount = Mathf.Clamp(
                player.Currencies[currencyID].Amount += player.Currencies[currencyID].Delta * Time.deltaTime,
                0f,
                player.Currencies[currencyID].Data.Max < 0 ? Mathf.Infinity : player.Currencies[currencyID].Data.Max);
        }
    }
    
}
