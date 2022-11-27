using Rebar.Util;
using UnityEngine;

/// <summary>
/// Sends a rotation impulse to a FxApplierRotate when triggered by FxPackageController.
/// </summary>
public class FxRotate : FxPackage {

	//FACTOR ATTRIBUTES
	[Range(0F, 150F)] public float DecayRate = 3F;
	public AnimationCurve ValueCurve = new AnimationCurve (new Keyframe (0F, 1F), new Keyframe (1F, 1F));

	//ANATOMY
	private FxApplierRotate applier;

	public override void SetUp (GameObject newAnchor = null) {

		base.SetUp(newAnchor);
		if (newAnchor == null) return;
		Anchor = newAnchor;

		if (newAnchor.GetComponent<FxApplierRotate>() != null) {
			applier = newAnchor.GetComponent<FxApplierRotate>();
		} else {
			applier = newAnchor.AddComponent<FxApplierRotate>();
			applier.SetUp(newAnchor);
		}

	}

	//If triggered, PASS ONCE through the value curve.
	public override void Trigger (GameObject newAnchor = null, float amplitude = 1f, float timeOffset = 0f) {
		base.Trigger(newAnchor, amplitude, timeOffset);
		applier.SetRotateFactor(SfxId, AnimationCurveUtilities.MultipliedCurve(ValueCurve, amplitude), DecayRate, false, amplitude, timeOffset);
	}

	//If toggled, PASS AND CONTINUE through the value curve.
	public override void Toggle (bool toggleState, GameObject newAnchor = null, float amplitude = 1f, float timeOffset = 0f) {
		base.Toggle(toggleState, newAnchor, amplitude, timeOffset);
		if (toggleState)
			applier.SetRotateFactor(SfxId, AnimationCurveUtilities.MultipliedCurve(ValueCurve, amplitude), DecayRate, true, amplitude, timeOffset);
		else
			applier.EndFactor(SfxId);
	}

}
