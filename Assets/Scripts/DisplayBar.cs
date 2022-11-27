using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class DisplayBar : MonoBehaviour {

    public RectTransform BarRT;

    public Vector2 emptyScale;
    public Vector2 fullScale;

    public void SetPct(float pct) {
        float clampPct = Mathf.Clamp01(pct);
        Vector2 lerpVec = Vector2.Lerp(emptyScale, fullScale, clampPct);
        BarRT.localScale = new Vector3(lerpVec.x, lerpVec.y, 1f);
    }

}
