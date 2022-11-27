using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Master Game Data")]
public class GameData : ScriptableObject {

    public float PlayerStartingHealth;
    public List<CurrencyData> Currencies;
    public List<AbilityData> Abilities;

    public AbilityData GetAbilityData(Ability ability) {
        return Abilities.First(a => a.ID == ability.Data.ID);
    }
}
