using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerEffectActions {

    private PlayerManager _manager;
    
    public PlayerManagerEffectActions(PlayerManager manager) {
        _manager = manager;
    }
    
    public void AddHealth (PlayerManager.PlayerID playerID, float addAmount) {
        PlayerInfo playerInfo = _manager.GetPlayerInfo(playerID);
        playerInfo.HealthCurrent = Mathf.Clamp(playerInfo.HealthCurrent + addAmount, 0, playerInfo.HealthMax);
    }
    
    public void AddHealthDelta (PlayerManager.PlayerID playerID, float addAmount) {
        PlayerInfo playerInfo = _manager.GetPlayerInfo(playerID);
        playerInfo.HealthDelta = playerInfo.HealthDelta + addAmount;
    }

    public void AddCurrency(PlayerManager.PlayerID playerID, CurrencyData currencyData, float addAmount) {
        PlayerInfo playerInfo = _manager.GetPlayerInfo(playerID);
        PlayerCurrency currency = playerInfo.Currencies[currencyData.ID];
        currency.Amount = Mathf.Clamp(currency.Amount + addAmount, 0, currencyData.Max);
    }

    public void AddCurrencyDelta(PlayerManager.PlayerID playerID, CurrencyData currencyData, float addAmount) {
        PlayerInfo playerInfo = _manager.GetPlayerInfo(playerID);
        PlayerCurrency currency = playerInfo.Currencies[currencyData.ID];
        currency.Delta = currency.Delta + addAmount;
    }
    
}
