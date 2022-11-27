using UnityEngine;

/// <summary>
/// Sends a color factor to FxApplierColor, which blends it into the base color of a target graphic.
/// </summary>
public class FxColor : FxPackage {

	//FACTOR ATTRIBUTES
	[Range(0F, 15F)] public float DecayRate = 3F;
	[Range(0F, 25F)] public float Strength = 1F;
	public Color Color = Color.black;

	//ANATOMY
	private FxApplierColor applier;

	public override void SetUp (GameObject newAnchor = null) {

		base.SetUp(newAnchor);
		if (newAnchor == null) return;
		Anchor = newAnchor;
 
		if (newAnchor.GetComponent<FxApplierColor>() != null) {
			applier = newAnchor.GetComponent<FxApplierColor>();
		} else {
			applier = newAnchor.AddComponent<FxApplierColor>();
			applier.SetUp(newAnchor);
		}

	}

	protected override void Update () {

		//If toggled on, set the color value based on a gradient.
		if (ToggleState) {
			Debug.LogWarning("Toggled application is not implemented for FxColor");
		}
		
		base.Update();

	}

	public override void Trigger (GameObject newAnchor = null, float amplitude = 1f, float timeOffset = 0f) {
		base.Trigger(newAnchor, amplitude, timeOffset);
		applier.AddColorFactor(SfxId, Strength * amplitude, DecayRate, Color, timeOffset);
	}
}
