using UnityEngine;

/// <summary>
/// Tracks scaling factors applied to an object, sums them, and sets the object's scale.
/// </summary>
public class FxApplierSizePulse : FxApplier {
	
	private GameObject anchor;
	private bool hasAnchor;

	private Vector3 baseScale = Vector3.zero;

	public override void SetUp (GameObject newAnchor = null) {
		if (newAnchor == null) return;
		anchor = newAnchor;
		hasAnchor = true;
		if (ActiveFactors.Count == 0) {
			baseScale = newAnchor.transform.localScale;
		}
	}

	public void SetPulseFactor (int sfxId, float newValue, float newDecayRate, AnimationCurve newEaseInCurve, float newWaveFrequency, bool newInvertX, bool toggleResetsWave = true, float timeOffset = 0f) {
		
		enabled = true;		// Enable this when it is interacted with.
		
		FxFactorSizePulse factor = (FxFactorSizePulse)FactorWithId(sfxId);
		if(factor != null) {
			factor.Value = newValue;
			if (toggleResetsWave)
				factor.ResetWaveTimer();
		} else {
			factor = new FxFactorSizePulse ();
			factor.SfxId = sfxId;
			factor.Value = newValue;
			factor.DecayRate = newDecayRate;
			factor.EaseInCurve = newEaseInCurve;
			factor.WaveFrequency = newWaveFrequency;
			factor.InvertX = newInvertX;
			factor.ApplyTimeOffset(timeOffset);
			ActiveFactors.Add(factor);
		}
	}

	private void Update () {
		ApplyPulse();
		AgeFactors();

		if (ActiveFactors.Count == 0) {	// Disable this when it is no longer processing factors.
			enabled = false;
		}
	}

	private void ApplyPulse () {
		if (!hasAnchor)
			return;
		
		Vector3 vec = VectorSum();
		vec = new Vector3(vec.x + 1, vec.y + 1, vec.z + 1);
		anchor.transform.localScale = new Vector3(baseScale.x * vec.x, baseScale.y * vec.y, baseScale.z * vec.z);
	}
	
	private Vector3 VectorSum () {
		Vector3 ret = Vector3.zero;
		foreach (FxFactor factor in ActiveFactors) {
			Vector3 fVec = ((FxFactorSizePulse) factor).ProcessedVector();
			ret = ret + fVec;
		}
		return ret;
	}

	public void SetNewBaseScale(Vector3 newBaseScale) {
		baseScale = newBaseScale;
	}
}

public class FxFactorSizePulse : FxFactor {

	public AnimationCurve EaseInCurve;
	public float WaveFrequency = 1F;
	protected float WaveTimer;

	public bool InvertX;

	public override void UpdateFactor () {
		base.UpdateFactor();
		WaveTimer += Time.deltaTime;
	}

	public Vector3 ProcessedVector () {
		float val = Value * EaseInCurve.Evaluate(Age) * Mathf.Cos(WaveTimer * WaveFrequency * Mathf.PI * 2F);
		return new Vector3(val * (InvertX ? -1 : 1), val, val);
	}

	public void ResetWaveTimer () {
		WaveTimer = 0F;
	}

}
