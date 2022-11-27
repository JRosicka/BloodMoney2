using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Game Data/Ability")]
public class AbilityData : ScriptableObject {
    public string ID;
    public string CurrencyID;
    public List<AbilityTier> Tiers;
    public List<AbstractAbilityEffect> Effects;
    
}

[Serializable]
public struct AbilityTier {
    public int Cost;
    public int EffectAmount;
}