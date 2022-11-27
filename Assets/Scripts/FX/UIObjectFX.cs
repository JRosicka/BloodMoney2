using System.Collections.Generic;
using System.Linq;
using Rebar.Util;
using UnityEngine;

/// <summary>
/// References a number of pre-made graphical effects that can be invoked anywhere within the game. Convenient for
/// calling attention to UI elements when interaction is being called for.
/// </summary>
public class UIObjectFX : MonoBehaviour {
    
    private static UIObjectFX instance;

    /// <summary>
    /// Holds a mapping from an EffectType to an FX Prefab or instantiated FXPackageController. If a controller is
    /// called for but does not exist, a new controller object will be instantiated and cached.
    /// </summary>
    private class UIObjectFXMappingInternal {
        public readonly string EffectID;
        private readonly GameObject _prefab;
        private readonly Transform _transform;
        private FxPackageController _controller;
        public FxPackageController Controller {
            get {
                if (!_transform) return null;
                if (!_controller) {
                    _controller = Instantiate(_prefab, _transform).GetComponent<FxPackageController>();
                }
                return _controller;
            }
        }

        public UIObjectFXMappingInternal(UIObjectFXMapping mapping, Transform parentTransform) {
            EffectID = mapping.EffectID;
            _prefab = mapping.Prefab;
            _transform = parentTransform;
        }
    }

    public List<UIObjectFXMapping> EffectMappings = new List<UIObjectFXMapping>();
    private List<UIObjectFXMappingInternal> _mappings;

    private void Awake() {
        instance = this;
    }
    
    private void Start() {
        _mappings = new List<UIObjectFXMappingInternal>();
        foreach (UIObjectFXMapping mapping in EffectMappings) {
            _mappings.Add(new UIObjectFXMappingInternal(mapping, transform));
        }
    }

    public static void DoEffect(string effectID, GameObject targetObject, float amplitude = 1f, float timeOffset = 0f) {
        if (!instance) return;
        SetTarget(effectID, targetObject, out UIObjectFXMappingInternal effect);
        effect.Controller.TriggerAll(amplitude);
    }

    public static void ToggleEffect(string effectID, GameObject targetObject, bool isOn, float amplitude = 1f,  float timeOffset = 0f) {
        if (!instance) return;
        SetTarget(effectID, targetObject, out UIObjectFXMappingInternal effect);
        effect.Controller.ToggleAll(isOn, amplitude: amplitude, timeOffset: timeOffset);
    }

    private static void SetTarget(string effectID, GameObject targetObject, out UIObjectFXMappingInternal effect) {
        effect = instance._mappings.FirstOrDefault(e => e.EffectID == effectID);
        if (effect == null) {
            Debug.Log("Error in {typeof(UIObjectFX)}: no effect found with {typeof(EffectType)} \"{effectType.ToString()}\"");
            return;
        }

        effect.Controller.gameObject.transform.position = targetObject.transform.position;
        effect.Controller.PackageAnchor = targetObject;
        effect.Controller.RefreshSetup();
    }

}
