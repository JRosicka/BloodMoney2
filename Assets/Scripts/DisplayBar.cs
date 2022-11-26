using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class DisplayBar : MonoBehaviour {

    public RectTransform BarRT;

    public void SetPct(float pct) {
        float clampPct = Mathf.Clamp01(pct);
        BarRT.localScale = new Vector3(clampPct, 1f);
    }

}
