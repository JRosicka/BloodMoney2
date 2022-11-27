using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Game Data/Ability")]
public class AbilityData : ScriptableObject {
    public string ID;
    public string CurrencyID;
    public List<AbilityTier> Tiers;
    public List<AbilityEffect> Effects;
    
}

[Serializable]
public struct AbilityTier {
    public int Cost;
    public int EffectAmount;
}

public abstract class AbilityEffect : ScriptableObject {
    public abstract void DoEffect(int effectAmount);
}

[CreateAssetMenu(fileName = "New Ability", menuName = "Game Data/Ability/Effects")]
public class AttackAbilityEffect : AbilityEffect {
    public override void DoEffect(int effectAmount) {
        Debug.Log("Attack!");
    }
}