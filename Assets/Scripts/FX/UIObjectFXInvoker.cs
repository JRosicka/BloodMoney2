using UnityEngine;

/// <summary>
/// A MonoBehaviour that conveniently invokes a standard FX from UIObjectFX. Useful for calling FX actions from code
/// or as part of an animation.
/// </summary>
public class UIObjectFXInvoker : MonoBehaviour {

    public GameObject TargetObject;
    public UIObjectFX.EffectType EffectType;
    public float EffectStrength = 1f;

    private void Start() {
        if (!TargetObject) {
            TargetObject = gameObject;
        }
    }
    
    public void InvokeEffect() {
        UIObjectFX.DoEffect(EffectType, TargetObject, EffectStrength);
    }

}
