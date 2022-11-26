using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo {
    public PlayerManager.PlayerID ID;

    public float HealthMax;
    public float HealthCurrent;
    public Dictionary<string, PlayerCurrency> Currencies;
    public bool CanUseAbility(IAbility ability) {
        return true; //TODO
    }

}
