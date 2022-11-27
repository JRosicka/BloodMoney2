using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Master Game Data")]
public class GameData : ScriptableObject {

    public float PlayerStartingHealth;
    public List<CurrencyData> Currencies;
    public List<AbilityData> Abilities;
}
