using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Translates an object to an offset of a base position set on awake.
/// </summary>
public class FxApplierTranslate : FxApplier {
	private GameObject anchor;
	private bool hasAnchor;

	private readonly List<FxTranslateFactor> translateFactors = new List<FxTranslateFactor> ();
	private readonly List<FxFactor> shakeFactors = new List<FxFactor> ();

	//SMOOTH TRANSLATE
	private bool hasSetBasePosition;
	private Vector3 basePosition = Vector3.zero;
	private Vector2 factorSum = Vector2.zero;
	
	float xPosPerlinSeed;
	float yPosPerlinSeed;
	
	public override void SetUp (GameObject newAnchor = null) {
		if (newAnchor == null) return;
		anchor = newAnchor;
		hasAnchor = true;
		
		// Set the base position, but only once per applier. There is currently no use case for
		// setting the base position more than once. 
		if (!hasSetBasePosition) {	
			hasSetBasePosition = true;
			basePosition = newAnchor.transform.localPosition;
		}
		
		xPosPerlinSeed = Random.Range(0f, 50f);
		yPosPerlinSeed = Random.Range(100f, 150f);
	}

	public void SetTranslateFactor (int sfxId, Vector2 directionVector, AnimationCurve valueCurve, float newDecayRate, bool loops, float amplitude = 1f, float timeOffset = 0f) {
		
		enabled = true; // Enable this when it is interacted with.
		
		FxTranslateFactor factor = (FxTranslateFactor)FactorWithId(sfxId);
		if(factor != null) {
			factor.DirectionVector = directionVector.normalized;
			factor.ValueCurve = valueCurve;
		} else {
			factor = new FxTranslateFactor (sfxId, valueCurve, directionVector.normalized, newDecayRate, loops, amplitude);
			factor.ApplyTimeOffset(timeOffset);
			translateFactors.Add(factor);
		}
	}

	public void SetShakeFactor (int sfxId, float newValue, float perlinSpeed, float newDecayRate, float amplitude = 1f, float timeOffset = 0f) {
		if (!this) return;
		enabled = true;	// Enable this when it is interacted with.
		
		FxFactor factor = FactorWithId(sfxId);
		if(factor != null) {
			factor.Value = newValue;
		} else {
			factor = new FxFactor {
				SfxId = sfxId,
				Value = newValue,
				Speed = perlinSpeed,
				DecayRate = newDecayRate,
				Amplitude = amplitude
			};
			factor.ApplyTimeOffset(timeOffset);
			shakeFactors.Add(factor);
		}
	}

	private void Update () {
		AgeFactors();
		ApplyTranslation();
		
		if (translateFactors.Count == 0 && shakeFactors.Count == 0) {	// Disable this when it is no longer processing factors.
			enabled = false;
		}
	}

	private void ApplyTranslation () {
		if (!hasAnchor)
			return;
		
		anchor.transform.localPosition = basePosition + (Vector3)TranslationSum() + new Vector3(0.5f - Mathf.PerlinNoise(Time.time * ShakePerlinSpeed() * 2f, xPosPerlinSeed), 0.5f - Mathf.PerlinNoise(Time.time * ShakePerlinSpeed() * 2f, yPosPerlinSeed)) * (ShakeSum() * 4.5f);
	}

	private Vector2 TranslationSum () {
		factorSum = Vector2.zero;
		foreach (FxTranslateFactor factor in translateFactors) {
			factorSum += factor.LastDisplacement;
		}
		return factorSum;
	}

	public void EndFactor (int sfxId) {
		foreach (FxTranslateFactor factor in translateFactors) {
			if (factor.SfxId == sfxId)
				factor.Deactivate();
		}
	}

	private float ShakeSum () {
		float shakeSum = 0F;
		foreach (FxFactor factor in shakeFactors) {
			shakeSum += factor.ProcessedValue();
		}
		return shakeSum;
	}

	private float ShakePerlinSpeed() {
		if (shakeFactors.Count == 0) return 0f;
		float perlinSum = 0f;
		foreach (FxFactor factor in shakeFactors) {
			perlinSum += factor.Speed;
		}
		return perlinSum / shakeFactors.Count;
	}

	protected override void AgeFactors () {
		FactorsToDelete.Clear();
		foreach (FxTranslateFactor factor in translateFactors) {
			factor.UpdateFactor();
			if (factor.GetShouldBeDeleted())
				FactorsToDelete.Add(factor);
		}
		foreach (var factor in FactorsToDelete.Cast<FxTranslateFactor>()) {
			translateFactors.Remove(factor);
		}

		foreach (FxFactor factor in shakeFactors) {
			factor.UpdateFactor();
			if (factor.GetShouldBeDeleted())
				FactorsToDelete.Add(factor);
		}
		foreach (FxFactor factor in FactorsToDelete) {
			shakeFactors.Remove(factor);
		}
	}

	protected override FxFactor FactorWithId (int id) {
		foreach (var factor in translateFactors.Cast<FxFactor>().Where(factor => factor.SfxId == id)) {
			return factor;
		}

		return shakeFactors.FirstOrDefault(factor => factor.SfxId == id);
	}

}

public class FxTranslateFactor : FxFactor {

	public AnimationCurve ValueCurve;
	public Vector2 DirectionVector;

	public Vector2 LastDisplacement = Vector2.zero;
	private readonly bool loops;
	private bool isOn = true;

	public FxTranslateFactor (int sfxId, AnimationCurve valueCurve, Vector2 directionVector, float decayRate, bool loops, float amplitude = 1f) {
		SfxId = sfxId;
		ValueCurve = valueCurve;
		DirectionVector = directionVector;
		Amplitude = amplitude;
		DecayRate = decayRate;
		this.loops = loops;
	}

	public override void UpdateFactor () {
		
		Age += Time.deltaTime;
		if (isOn) {
			LastDisplacement = DisplacementFromCurve();
		} else {
			LastDisplacement = LastDisplacement.normalized * (LastDisplacement.magnitude * Mathf.Exp(-DecayRate * Time.deltaTime));
			if (LastDisplacement.magnitude < DeletionThreshold)
				ShouldBeDeleted = true;
		}

		//Handle LOOP (toggle) vs PASS ONCE (trigger) behavior.
		if (Age > ValueCurve.keys[ValueCurve.keys.Length - 1].time) {
			if (loops)
				Age = 0F;
			else
				isOn = false;
		}
		
	}

	public void Deactivate () {
		isOn = false;
	}

	private Vector2 DisplacementFromCurve () {
		return DirectionVector * (ValueCurve.Evaluate(Age) * Amplitude);
	}

}
