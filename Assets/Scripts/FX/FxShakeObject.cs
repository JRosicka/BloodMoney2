using UnityEngine;

/// <summary>
/// Lets an FX Package apply translational and rotational shake to an object.
/// </summary>
public class FxShakeObject : FxPackage {

	//FACTOR ATTRIBUTES
	[Header("Translate")]
	[Range(0F, 50f)] public float DecayRate = 3f;
	[Range(0F, 100f)] public float TranslationMultiplier = 5f;
	[Header("Rotate")]
	[Range(0F, 100f)] public float RotationMultiplier;
	public float PerlinSpeed = 10f;
	public AnimationCurve ValueCurve = new AnimationCurve (new Keyframe (0f, 1f), new Keyframe (1f, 1f)); 

	//ANATOMY
	private FxApplierTranslate applier;
	private FxApplierRotate rotApplier;

	public override void SetUp (GameObject newAnchor = null) {
		
		base.SetUp(newAnchor);
		if (newAnchor == null) return;
		Anchor = newAnchor;

		if (newAnchor.GetComponent<FxApplierTranslate>() != null) {
			applier = newAnchor.GetComponent<FxApplierTranslate>();
		} else {
			applier = newAnchor.AddComponent<FxApplierTranslate>();
		}
		
		if (newAnchor.GetComponent<FxApplierRotate>() != null) {
			rotApplier = newAnchor.GetComponent<FxApplierRotate>();
		} else {
			rotApplier = newAnchor.AddComponent<FxApplierRotate>();
		}
		applier.SetUp(newAnchor);
		rotApplier.SetUp(newAnchor);

	}

	protected override void Update () {

		//If toggled on, set the shake value based on the curve.
		if (ToggleState) {
			applier.SetShakeFactor(SfxId, ValueCurve.Evaluate(Controller.Timer) * TranslationMultiplier * LastSetAmplitude, PerlinSpeed, DecayRate);
			rotApplier.SetShakeFactor(SfxId, ValueCurve.Evaluate(Controller.Timer) * RotationMultiplier * LastSetAmplitude, PerlinSpeed, DecayRate);
		}
		
		base.Update();

	}

	public override void Trigger (GameObject newAnchor = null, float amplitude = 1, float timeOffset = 0f) {
		base.Trigger(newAnchor, amplitude, timeOffset);
		applier.SetShakeFactor(SfxId, ValueCurve.Evaluate(0F) * TranslationMultiplier * amplitude, PerlinSpeed, DecayRate);
		rotApplier.SetShakeFactor(SfxId, ValueCurve.Evaluate(0F) * RotationMultiplier * amplitude, PerlinSpeed, DecayRate);
	}

}
