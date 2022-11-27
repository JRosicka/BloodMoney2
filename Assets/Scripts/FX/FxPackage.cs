using UnityEngine;

/// <summary>
/// Parent class for FxPackages. Handles basic anchor setting and on/off toggle state.
/// </summary>
public class FxPackage : MonoBehaviour {

	[HideInInspector] public FxPackageController Controller;
	[HideInInspector] public bool ToggleState;

	protected GameObject Anchor;

	public static int NextFxId;
	protected int SfxId = -1;

	protected float LastSetAmplitude = 1f;

	public virtual void Trigger (GameObject newAnchor = null, float amplitude = 1f, float timeOffset = 0f) {
		
		// Enable this when it is interacted with.
		enabled = true;

		IncrementSfxId();

		if (newAnchor)
			Anchor = newAnchor;
		
	}

	public virtual void Toggle (bool toggleState, GameObject newAnchor = null, float amplitude = 1f, float timeOffset = 0f) {
		
		// Enable this when it is interacted with.
		enabled = true;
		
		if (!ToggleState && toggleState) {
			IncrementSfxId();
		}

		ToggleState = toggleState;
		LastSetAmplitude = amplitude;
		if (newAnchor)
			Anchor = newAnchor;
		
	}

	protected virtual void Update() {
		
		// Disable this if it isn't doing anything.
		if (!ToggleState) {
			enabled = false;
		}

	}

	public virtual void SetUp (GameObject newAnchor = null) {

		// Called by the SFXPackageController's Start() function.
		// Enable this when it is interacted with.
        enabled = true;

	}

	private void IncrementSfxId () {

		SfxId = NextFxId;
		NextFxId++;

	}

}
