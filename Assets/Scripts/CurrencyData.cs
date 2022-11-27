using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Currency", menuName = "Game Data/Currency")]
public class CurrencyData : ScriptableObject {

    public string ID;
    public string DisplayName;
    public int Max;

}
