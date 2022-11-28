using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Game Data/Ability/Effects/Attack")]
public class AttackAbilityEffect : AbstractAbilityEffect {
    public SoundCall AttackSound;
    public override void DoEffect(PlayerManager.PlayerID playerID, int effectAmount) {
        Debug.Log("Attack!");
        GameManager.Instance.PlayerManager.EffectActions.DealDamage(PlayerManager.OpponentOf(playerID), effectAmount);
        SoundManager.thisSoundManager.PlaySound(AttackSound, null);
    }
}