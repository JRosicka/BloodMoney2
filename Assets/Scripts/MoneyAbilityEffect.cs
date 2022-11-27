using UnityEngine;

[CreateAssetMenu(fileName = "New Money", menuName = "Game Data/Ability/Effects/Money")]
public class MoneyAbilityEffect : AbstractAbilityEffect {
    public CurrencyData Currency;
    public override void DoEffect(PlayerManager.PlayerID playerID, int effectAmount) {
        Debug.Log("Money!");
        GameManager.Instance.PlayerManager.EffectActions.AddCurrency(playerID, Currency, effectAmount);
    }
}