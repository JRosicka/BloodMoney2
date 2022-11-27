using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Rotates an object by summing rotation factors.
/// </summary>
public class FxApplierRotate : FxApplier {
	
	private GameObject anchor;
	private bool hasAnchor;

	private readonly List<FxRotateFactor> rotateFactors = new List<FxRotateFactor> ();
	private readonly List<FxFactor> shakeFactors = new List<FxFactor>();
	
	//SMOOTH Rotate
	private float factorSum;
	private float lastAppliedRotation;
	
	float rotPerlinSeed;

	public override void SetUp (GameObject newAnchor = null) {
		if (newAnchor == null) return;
		anchor = newAnchor;
		hasAnchor = anchor != null;
		rotPerlinSeed = Random.Range(200f, 250f);
	}

	public void SetRotateFactor (int sfxId, AnimationCurve valueCurve, float newDecayRate, bool loops, float amplitude = 1f, float timeOffset = 0f) {
		
		enabled = true;	// Enable this when it is interacted with.
		
		FxRotateFactor factor = (FxRotateFactor)FactorWithId(sfxId);
		if(factor != null) {
			factor.ValueCurve = valueCurve;
		} else {
			factor = new FxRotateFactor (sfxId, valueCurve, newDecayRate, loops, amplitude);
			factor.ApplyTimeOffset(timeOffset);
			rotateFactors.Add(factor);
		}
	}
	
	public void SetShakeFactor (int sfxId, float newValue, float perlinSpeed, float newDecayRate, float amplitude = 1f, float timeOffset = 0f) {
		
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
		ApplyRotation();

		if (rotateFactors.Count == 0 && shakeFactors.Count == 0) {	// Disable this when it is no longer processing factors.
			enabled = false;
		}
	}

	private void ApplyRotation () {
		if (!hasAnchor)
			return;
		
		if (Math.Abs(lastAppliedRotation) > Single.Epsilon)
			anchor.transform.Rotate(0f, 0f, -lastAppliedRotation);

		if (shakeFactors.Count > 0 || rotateFactors.Count > 0)
			lastAppliedRotation = RotationSum();
		else
			lastAppliedRotation = 0;
			
		if (Math.Abs(lastAppliedRotation) > Single.Epsilon)
			anchor.transform.Rotate(0f, 0f, lastAppliedRotation);

	}

	public float RotationSum () {
		factorSum = 0f;
		foreach (FxRotateFactor factor in rotateFactors) {
			factorSum += factor.LastDisplacement;
		}
		float shakeVal = (0.5f - Mathf.PerlinNoise(Time.time * ShakePerlinSpeed(), rotPerlinSeed)) * ShakeSum();
		return factorSum + shakeVal;
	}

	public void EndFactor (int sfxId) {
		foreach (FxRotateFactor factor in rotateFactors) {
			if (factor.SfxId == sfxId)
				factor.Deactivate();
		}
	}
	
	public float ShakeSum () {
		float shakeSum = 0F;
		foreach (FxFactor factor in shakeFactors) {
			shakeSum += factor.ProcessedValue();
		}
		return shakeSum;
	}

	public float ShakePerlinSpeed() {
		if (shakeFactors.Count == 0) return 0f;
		float perlinSum = 0f;
		foreach (FxFactor factor in shakeFactors) {
			perlinSum += factor.Speed;
		}
		return perlinSum / shakeFactors.Count;
	}

	override protected void AgeFactors () {
		FactorsToDelete.Clear();
		foreach (FxRotateFactor factor in rotateFactors) {
			factor.UpdateFactor();
			if (factor.GetShouldBeDeleted())
				FactorsToDelete.Add(factor);
		}
		foreach (var fxFactor in FactorsToDelete) {
			var factor = (FxRotateFactor) fxFactor;
			rotateFactors.Remove(factor);
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
		foreach (FxRotateFactor factor in rotateFactors) {
			if (factor.SfxId == id)
				return factor;
		}
		foreach (FxRotateFactor factor in rotateFactors) {
			if (factor.SfxId == id)
				return factor;
		}
		return null;
	}

}

public class FxRotateFactor : FxFactor {

	public AnimationCurve ValueCurve;
	private float LastKeyFrameTime;

	public float LastDisplacement;
	protected readonly bool Loops;
	protected bool IsOn = true;

	public FxRotateFactor (int sfxId, AnimationCurve valueCurve, float decayRate, bool loops, float amplitude = 1f) {
		SfxId = sfxId;
		ValueCurve = valueCurve;
		DecayRate = decayRate;
		Loops = loops;
		Amplitude = amplitude;
		var keys = ValueCurve.keys;
		LastKeyFrameTime = keys[keys.Length - 1].time;
	}

	public override void UpdateFactor () {
		
		Age += Time.deltaTime;
		if (IsOn) {
			LastDisplacement = ValueCurve.Evaluate(Age);
		} else {
			LastDisplacement = LastDisplacement * Mathf.Exp(-DecayRate * Time.deltaTime);
			if (Mathf.Abs(LastDisplacement) < DeletionThreshold)
				ShouldBeDeleted = true;
		}

		// Handle LOOP (toggle) vs PASS ONCE (trigger) behavior.
		if (Age > LastKeyFrameTime) {
			IsOn = IsOn && Loops;
		}
		
	}

	public void Deactivate () {
		IsOn = false;
	}

}
