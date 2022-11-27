using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff", menuName = "Game Data/Buff")]
public class BuffData : ScriptableObject {
    
    public string ID;
    public Sprite ArtSprite;
    public Sprite BackgroundSprite;
    public float Duration;
    public List<AbstractAbilityEffect> StartEffects;
    public List<AbstractAbilityEffect> Effects;
    public float EffectPeriod = 0.1f;
    public List<AbstractAbilityEffect> EndEffects;

    public bool CausesGuard;

}