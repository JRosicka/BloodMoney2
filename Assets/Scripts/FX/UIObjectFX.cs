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

    public enum EffectType {
        
        // Combined rotational wiggle and scale pulse. Use to attract the eye to the next desired input.
        AttentionShake,    
        
        // Instant white flash, then fade. Use to attract attention to lower-opacity objects.
        WhiteFlash,
        
        // Size Pulse. Indicate that a button has been pressed.
        ButtonClickSuccess,
        
        // Shake and rotation that simulates an "I can't do that" waggle. Indicate that an action can't be taken right now.
        ButtonClickFailure,  
        
        // Shake, size pulse, and a small upward bounce. Used on a currency icon when the associated currency goes up.
        CollectionBounce, 
        
        // Toggled downscaling and rotational wiggle. Used to indicate a card is interactable during Edit Mode.
        EditModeWiggle,
        
        // Brief size pulse. Used on a club collection bar to indicate the club is upgradeable.
        UpgradablePulse,
        
        // Upward Bounce, Size Pulse, and Wiggle. Used when a card transitions into another card on the inventory.
        CardRefreshPulse,
        
        // Particle effect performed around a quest tile when it is claimable.
        QuestClaimableParticles
        
    }

    /// <summary>
    /// Holds a mapping from an EffectType to an FX Prefab or instantiated FXPackageController. If a controller is
    /// called for but does not exist, a new controller object will be instantiated and cached.
    /// </summary>
    private class UIObjectFXMappingInternal {
        public readonly EffectType EffectType;
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
            EffectType = mapping.EffectType;
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

    public static void DoEffect(EffectType effectType, GameObject targetObject, float amplitude = 1f, float timeOffset = 0f) {
        if (!instance) return;
        SetTarget(effectType, targetObject, out UIObjectFXMappingInternal effect);
        effect.Controller.TriggerAll(amplitude);
    }

    public static void ToggleEffect(EffectType effectType, GameObject targetObject, bool isOn, float amplitude = 1f,  float timeOffset = 0f) {
        if (!instance) return;
        SetTarget(effectType, targetObject, out UIObjectFXMappingInternal effect);
        effect.Controller.ToggleAll(isOn, amplitude: amplitude, timeOffset: timeOffset);
    }

    private static void SetTarget(EffectType effectType, GameObject targetObject, out UIObjectFXMappingInternal effect) {
        effect = instance._mappings.FirstOrDefault(e => e.EffectType == effectType);
        if (effect == null) {
            Debug.Log("Error in {typeof(UIObjectFX)}: no effect found with {typeof(EffectType)} \"{effectType.ToString()}\"");
            return;
        }

        effect.Controller.gameObject.transform.position = targetObject.transform.position;
        effect.Controller.PackageAnchor = targetObject;
        effect.Controller.RefreshSetup();
    }

}
