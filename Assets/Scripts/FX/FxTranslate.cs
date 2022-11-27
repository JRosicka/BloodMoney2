using UnityEngine;

/// <summary>
/// Issues a translation factor to an FxApplierTranslate when triggered or toggled by an FxPackageController
/// </summary>
public class FxTranslate : FxPackage {

	//FACTOR ATTRIBUTES
	[Range(0F, 150F)] public float DecayRate = 3F;
	public Vector2 DirectionVector = Vector2.up;
	public AnimationCurve ValueCurve = new AnimationCurve (new Keyframe (0F, 1F), new Keyframe (1F, 1F));

	//ANATOMY
	private FxApplierTranslate applier;

	public override void SetUp (GameObject newAnchor = null) {

		base.SetUp(newAnchor);
		if (newAnchor == null) return;
		Anchor = newAnchor;

		if (newAnchor.GetComponent<FxApplierTranslate>() != null) {
			applier = newAnchor.GetComponent<FxApplierTranslate>();
		} else {
			applier = newAnchor.AddComponent<FxApplierTranslate>();
			applier.SetUp(newAnchor);
		}

	}

	//If triggered, PASS ONCE through the value curve.
	public override void Trigger (GameObject newAnchor = null, float amplitude = 1f, float timeOffset = 0f) {
		base.Trigger(newAnchor, amplitude, timeOffset);
		applier.SetTranslateFactor(SfxId, DirectionVector, ValueCurve, DecayRate, false, amplitude, timeOffset);
	}

	//If toggled, LOOP through the value curve.
	public override void Toggle (bool toggleState, GameObject newAnchor = null, float amplitude = 1f, float timeOffset = 0f) {
		base.Toggle(toggleState, newAnchor, amplitude, timeOffset);
		if (toggleState)
			applier.SetTranslateFactor(SfxId, DirectionVector, ValueCurve, DecayRate, true, amplitude, timeOffset);
		else
			applier.EndFactor(SfxId);
	}

}
