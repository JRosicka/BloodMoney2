using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff", menuName = "Game Data/Buff")]
public class BuffData : ScriptableObject {
    public string ID;
    public float Duration;
    public List<AbstractAbilityEffect> Effects;
    public float EffectPeriod = 0.1f;
    
}