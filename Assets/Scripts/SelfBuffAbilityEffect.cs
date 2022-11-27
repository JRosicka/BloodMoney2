using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Game Data/Ability/Effects/Self Buff")]
public class SelfBuffAbilityEffect : AbstractAbilityEffect {
    public BuffData Buff;
    
    public override void DoEffect(PlayerManager.PlayerID playerID, int effectAmount) {
        Debug.Log("Self Buff with " + Buff.ID + "!");
        GameManager.Instance.PlayerManager.EffectActions.AddBuff(playerID, Buff);
    }
}