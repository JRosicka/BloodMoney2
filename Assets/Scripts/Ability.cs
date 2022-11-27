/// <summary>
/// A thing that a button can do. There can be all sorts of types!
/// </summary>
public class Ability {
    public string CurrencyID { get; }
    public AbilityData Data { get; }
    
    /// <summary>
    /// Increments each time we use the ability
    /// </summary>
    private int _currentTier;

    /// <summary>
    /// Whether we need to wait to use the ability because it is on cooldown
    /// </summary>
    public bool AbilityCooldownActive() {
        return false;  // TODO
    }

    public int CostToUse() {
        return 0;    // TODO
    }

    public void BuyAndUseAbility() {
        AbilityTier tier = Data.Tiers[_currentTier];
        // Buy
        // TODO
        
        // Use
        Data.Effects.ForEach(e => e.DoEffect(tier.EffectAmount));
        
        // Increment
        if (_currentTier < Data.Tiers.Count - 1) {
            _currentTier++;
        }
    }

    public Ability(AbilityData data) {
        Data = data;
    }
}