using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInfo {
    public PlayerManager.PlayerID ID;

    public float HealthMax;
    public float HealthCurrent;
    public Dictionary<string, PlayerCurrency> Currencies;
    public List<Ability> Abilities;

    private Ability GetAbility(string id) => Abilities.First(a => a.Data.ID == id);
    
    public bool CanUseAbility(string abilityID) {
        Ability ability = GetAbility(abilityID);
        return CanAffordAbility(ability) && !ability.AbilityCooldownActive();
    }

    private bool CanAffordAbility(Ability ability) {
        return ability.CostToUse() <= Currencies[ability.CurrencyID].Amount;
    }

    public bool TryUseAbility(string abilityID) {
        if (!CanUseAbility(abilityID)) return false;
        
        GetAbility(abilityID).BuyAndUseAbility();
        return true;
    }
}
