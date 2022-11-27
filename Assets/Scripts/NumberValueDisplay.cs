using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberValueDisplay : MonoBehaviour {

    public TextMeshProUGUI Label;
    public TextMeshProUGUI Value;
    public TextMeshProUGUI Delta;

    private int lastNumerator;
    private int lastDenominator;
    private float lastChangePerSecond;

    public string LabelPattern;
    public string ValuePatternWithMax;
    public string ValuePatternWithoutMax;
    public string DeltaPattern;

    public void SetLabel(string newLabel) {
        Label.text = string.Format(LabelPattern, newLabel);
    }
    
    public void SetValueNumerator(int newNumerator) {
        lastNumerator = newNumerator;
    }

    public void SetValueDenominator(int newDenominator) {
        lastDenominator = newDenominator;
    }

    public void SetDelta(float newChangePerSecond) {
        lastChangePerSecond = newChangePerSecond;
    }

    private void Update() {
        if (lastDenominator > 0) {
            Value.text = string.Format(ValuePatternWithMax, lastNumerator, lastDenominator);
        }
        else {
            Value.text = string.Format(ValuePatternWithoutMax, lastNumerator);
        }
        string changeSign = lastChangePerSecond < 0 ? "-" : "+";
        Delta.text = string.Format(DeltaPattern, changeSign + lastChangePerSecond.ToString("F2"));
    }

}
