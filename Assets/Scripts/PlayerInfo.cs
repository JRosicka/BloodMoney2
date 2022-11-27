using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInfo {
    public PlayerManager.PlayerID ID;

    public float HealthMax;
    public float HealthCurrent;
    public float HealthDelta;
    public Dictionary<string, PlayerCurrency> Currencies;
    public List<Ability> Abilities;

    
    // Buffs
    public List<PlayerBuff> ActiveBuffs = new List<PlayerBuff>();
    public bool HasGuard => ActiveBuffs.Any(e => e.Data.CausesGuard);
    
    public delegate void BuffCreatedDelegate (PlayerBuff buff);
    public event BuffCreatedDelegate OnBuffCreated;
    public event BuffCreatedDelegate OnBuffDestroyed;
    

    public Ability GetAbility(string id) => Abilities.First(a => a.Data.ID == id);
    
    public bool TryUseAbility(string abilityID) {
        Ability ability = GetAbility(abilityID);
        if (!CanUseAbility(ability)) return false;
        
        // Buy it
        Currencies[ability.Data.Currency.ID].Spend(ability.CostToUse());
        
        ability.UseAbility();
        return true;
    }

    private bool CanUseAbility(Ability ability) {
        return CanAffordAbility(ability) && !ability.AbilityCooldownActive();
    }

    private bool CanAffordAbility(Ability ability) {
        return ability.CostToUse() <= Currencies[ability.Data.Currency.ID].Amount;
    }

    public void AddBuff(BuffData buff) {
        PlayerBuff existingBuff = GrabExistingBuff(buff);
        if (existingBuff != null) {
            existingBuff.Refresh();
            return;
        }
        PlayerBuff newBuff = new PlayerBuff(buff);
        ActiveBuffs.Add(newBuff);
        if (OnBuffCreated != null) OnBuffCreated(newBuff);
    }

    public void DestroyBuff(BuffData buff) {
        PlayerBuff existingBuff = GrabExistingBuff(buff);
        if (existingBuff != null) {
            ActiveBuffs.Remove(existingBuff);
            if (OnBuffDestroyed != null) OnBuffDestroyed(existingBuff);
        }
    }

    private PlayerBuff GrabExistingBuff(BuffData buff) {
        foreach (PlayerBuff activeBuff in ActiveBuffs) {
            if (activeBuff.Data.ID == buff.ID) return activeBuff;
        }
        return null;
    }
    
    public PlayerBuff GrabExistingBuff(string buffID) {
        foreach (PlayerBuff activeBuff in ActiveBuffs) {
            if (activeBuff.Data.ID == buffID) return activeBuff;
        }
        return null;
    }

    public void TakeDamage(float damageAmount) {
        if (HasGuard) {
            damageAmount *= 0.25f;
        }
        HealthCurrent = Mathf.Clamp(HealthCurrent - damageAmount, 0, HealthMax);
    }

    public void Heal(float healAmount) {
        HealthCurrent = Mathf.Clamp(HealthCurrent + healAmount, 0, HealthMax);
    }
    
    public void AddHealthDelta (float addAmount) {
        HealthDelta = HealthDelta + addAmount;
    }

    public void AddCurrency(CurrencyData currencyData, float addAmount) {
        PlayerCurrency currency = Currencies[currencyData.ID];
        currency.Amount = Mathf.Clamp(currency.Amount + addAmount, 0, currencyData.Max > 0 ? currencyData.Max : Mathf.Infinity);
    }

    public void AddCurrencyDelta(CurrencyData currencyData, float addAmount) {
        PlayerCurrency currency = Currencies[currencyData.ID];
        currency.Delta = currency.Delta + addAmount;
    }
    
}

