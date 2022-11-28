using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityInfoPanelDisplay : MonoBehaviour {

    public TextMeshProUGUI Title;
    public Image PriceCurrencyIcon;
    public TextMeshProUGUI PriceAmount;
    public TextMeshProUGUI Description;

    public List<StringReplacePair> StringReplacePairs;

    public void SetAbility(Ability ability) {
        Title.text = ability.Data.ID;
        PriceCurrencyIcon.sprite = ability.Data.Currency.Sprite;
        PriceAmount.text = ability.CostToUse().ToString();
        string descriptionString = ability.DescriptionAtCurrentTier();
        foreach (StringReplacePair replacePair in StringReplacePairs) {
            descriptionString = descriptionString.Replace(replacePair.OldString, replacePair.NewString);
        }
        Description.text = descriptionString;
    }
    
}

[System.Serializable]
public class StringReplacePair {
    public string OldString;
    public string NewString;
}
