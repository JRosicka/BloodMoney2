/// <summary>
/// A thing that a button can do. There can be all sorts of types!
/// </summary>
public class Ability {
    public AbilityData Data { get; }

    private PlayerManager.PlayerID _playerID;
    
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
        return Data.Tiers[_currentTier].Cost;
    }

    public void UseAbility() {
        AbilityTier tier = Data.Tiers[_currentTier];
        
        // Use
        Data.Effects.ForEach(e => e.DoEffect(_playerID, tier.EffectAmount));
        
        // Increment
        if (_currentTier < Data.Tiers.Count - 1) {
            _currentTier++;
        }
    }

    public Ability(AbilityData data, PlayerManager.PlayerID playerID) {
        Data = data;
        _playerID = playerID;
    }
}