using UnityEngine;

/// <summary>
/// Sends an FxFactorSizePulse to an applier, causing either a pulse in size or in squash-and-stretch-style bounce.
/// </summary>
public class FxSizePulse : FxPackage {

	//FACTOR ATTRIBUTES
	[Range(0F, 50F)] public float DecayRate = 3F;
	[Range(-5F, 5F)] public float ValueMultiplier = 0.6F;
	public AnimationCurve ValueCurve = new AnimationCurve (new Keyframe (0F, 1F), new Keyframe (1F, 1F));
	[Header("Wave Attributes")]
	public AnimationCurve EaseInCurve = new AnimationCurve (new Keyframe (0F, 0F, 4F, 4F), new Keyframe (0.45F, 1F, 0F, 0F));
	[Range(0F, 10F)] public float WaveFrequency = 1F;
	public bool ToggleResetsWave = true;

	[Header("Inversion (Squash & Stretch)")]
	public bool InvertX;
	
	//ANATOMY
	private FxApplierSizePulse applier;

	public override void SetUp (GameObject newAnchor = null) {

		base.SetUp(newAnchor);
		if (newAnchor == null) return;
		Anchor = newAnchor;
 
		applier = newAnchor.GetComponent<FxApplierSizePulse>() != null ? newAnchor.GetComponent<FxApplierSizePulse>() : newAnchor.AddComponent<FxApplierSizePulse>();
		applier.SetUp(newAnchor);

	}

	protected override void Update () {

		//If toggled on, set the shake value based on the curve.
		if (ToggleState) {
			applier.SetPulseFactor(SfxId, ValueCurve.Evaluate(Controller.Timer) * ValueMultiplier * LastSetAmplitude, DecayRate, EaseInCurve, WaveFrequency, InvertX, ToggleResetsWave);
		}
		
		base.Update();

	}

	public override void Trigger (GameObject newAnchor = null, float amplitude = 1f, float timeOffset = 0f) {
		base.Trigger(newAnchor, amplitude, timeOffset);
		applier.SetPulseFactor(SfxId, ValueCurve.Evaluate(0F) * ValueMultiplier * amplitude, DecayRate, EaseInCurve, WaveFrequency, InvertX, timeOffset: timeOffset);
	}
}
