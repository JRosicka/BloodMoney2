using UnityEngine;

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

    private float _timeOfLastUsage = Mathf.NegativeInfinity;
    
    /// <summary>
    /// Whether we need to wait to use the ability because it is on cooldown
    /// </summary>
    public bool AbilityCooldownActive() {
        return _timeOfLastUsage + Data.CooldownTime > Time.time;
    }

    public int CostToUse() {
        return Data.Tiers[_currentTier].Cost;
    }

    public string DescriptionAtCurrentTier() {
        return Data.Tiers[_currentTier].DescriptionString;
    }

    public void UseAbility() {
        // Use
        Data.Tiers[_currentTier].Effects.ForEach(e => e.Effect.DoEffect(_playerID, e.EffectAmount));
        
        // Increment
        if (_currentTier < Data.Tiers.Count - 1) {
            _currentTier++;
        }
        
        // Set cooldown time
        _timeOfLastUsage = Time.time;
    }

    public Ability(AbilityData data, PlayerManager.PlayerID playerID) {
        Data = data;
        _playerID = playerID;
    }
}