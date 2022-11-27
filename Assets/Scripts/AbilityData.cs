using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Game Data/Ability")]
public class AbilityData : ScriptableObject {
    public string ID;
    public string CurrencyID;
    public float CooldownTime;
    public List<AbilityTier> Tiers;
}

[Serializable]
public struct AbilityTier {
    public int Cost;
    public List<NumberedAbilityEffect> Effects;
}

[Serializable]
public struct NumberedAbilityEffect {
    public int EffectAmount;
    public AbstractAbilityEffect Effect;
}