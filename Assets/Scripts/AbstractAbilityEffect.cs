using UnityEngine;

public abstract class AbstractAbilityEffect : ScriptableObject {
    public abstract void DoEffect(PlayerManager.PlayerID playerID, int effectAmount);
}