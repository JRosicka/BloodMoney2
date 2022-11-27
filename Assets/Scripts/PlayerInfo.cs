using System.Collections.Generic;
using System.Linq;

public class PlayerInfo {
    public PlayerManager.PlayerID ID;

    public float HealthMax;
    public float HealthCurrent;
    public Dictionary<string, PlayerCurrency> Currencies;
    public List<Ability> Abilities;

    private Ability GetAbility(string id) => Abilities.First(a => a.Data.ID == id);
    
    public bool TryUseAbility(string abilityID) {
        Ability ability = GetAbility(abilityID);
        if (!CanUseAbility(ability)) return false;
        
        // Buy it
        Currencies[ability.Data.CurrencyID].Spend(ability.CostToUse());
        
        ability.UseAbility();
        return true;
    }

    private bool CanUseAbility(Ability ability) {
        return CanAffordAbility(ability) && !ability.AbilityCooldownActive();
    }

    private bool CanAffordAbility(Ability ability) {
        return ability.CostToUse() <= Currencies[ability.Data.CurrencyID].Amount;
    }
}
