using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerEffectActions {

    private PlayerManager _manager;
    
    public PlayerManagerEffectActions(PlayerManager manager) {
        _manager = manager;
    }
    
    public void DealDamage (PlayerManager.PlayerID playerID, float damageAmount) {
        PlayerInfo playerInfo = _manager.GetPlayerInfo(playerID);
        playerInfo.TakeDamage(damageAmount);
    }

    public void Heal(PlayerManager.PlayerID playerID, float healAmount) {
        PlayerInfo playerInfo = _manager.GetPlayerInfo(playerID);
        playerInfo.Heal(healAmount);
    }
    
    public void AddHealthDelta (PlayerManager.PlayerID playerID, float addAmount) {
        PlayerInfo playerInfo = _manager.GetPlayerInfo(playerID);
        playerInfo.AddHealthDelta(addAmount);
    }

    public void AddCurrency(PlayerManager.PlayerID playerID, CurrencyData currencyData, float addAmount) {
        PlayerInfo playerInfo = _manager.GetPlayerInfo(playerID);
        playerInfo.AddCurrency(currencyData, addAmount);
    }

    public void AddCurrencyDelta(PlayerManager.PlayerID playerID, CurrencyData currencyData, float addAmount) {
        PlayerInfo playerInfo = _manager.GetPlayerInfo(playerID);
        playerInfo.AddCurrencyDelta(currencyData, addAmount);
    }

    public void AddBuff(PlayerManager.PlayerID playerID, BuffData buff) {
        PlayerInfo playerInfo = _manager.GetPlayerInfo(playerID);
        playerInfo.AddBuff(buff);
    } 
    
}
