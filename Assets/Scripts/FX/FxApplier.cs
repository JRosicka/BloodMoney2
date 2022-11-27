using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Points to a gameObject and calculates/manages factors that influence that object.
/// </summary>
public class FxApplier : MonoBehaviour {

	protected List<FxFactor> ActiveFactors = new List<FxFactor> ();
	protected List<FxFactor> FactorsToDelete = new List<FxFactor> ();

	protected virtual void AgeFactors () {
		FactorsToDelete.Clear();
		foreach (FxFactor factor in ActiveFactors) {
			factor.UpdateFactor();
			if (factor.GetShouldBeDeleted())
				FactorsToDelete.Add(factor);
		}
		foreach (FxFactor factor in FactorsToDelete) {
			ActiveFactors.Remove(factor);
		}
	}

	protected virtual FxFactor FactorWithId (int id) {
		foreach (FxFactor factor in ActiveFactors) {
			if (factor.SfxId == id)
				return factor;
		}
		return null;
	}

	protected float FactorSum () {
		float ret = 0F;
		foreach (FxFactor factor in ActiveFactors) {
			ret += factor.ProcessedValue();
		}
		return ret;
	}

	public float FactorProduct () {
		float ret = 1F;
		foreach (FxFactor factor in ActiveFactors) {
			ret *= (1F + factor.ProcessedValue());
		}
		return ret;
	}

	public virtual void SetUp(GameObject newAnchor = null) {
		
	}

	public void ClearFactors() {
		ActiveFactors.Clear();
	}

}

public class FxFactor {

	public int SfxId;	//The ID of the SFX Component this came from.
	public float Value;
	protected float Age;
	public float DecayRate = 3F;
	public float Speed = 1f;
	protected const float DeletionThreshold = 0.01F;
	protected float LastAgeAboveDeletionThreshold = -1F;
	protected const float TimeBeforeDeletion = 3F;
	protected bool ShouldBeDeleted;
	public float Amplitude = 1f;

	public virtual void UpdateFactor () {

		Age += Time.deltaTime;
		DecayValue();
		if (Mathf.Abs(Value) > DeletionThreshold)
			LastAgeAboveDeletionThreshold = Age;
		if (Age - TimeBeforeDeletion > LastAgeAboveDeletionThreshold)
			ShouldBeDeleted = true;

	}

	public void ApplyTimeOffset(float timeOffset) {
		Age += timeOffset;
	}
	
	public bool GetShouldBeDeleted () {
		return ShouldBeDeleted;
	}

	public virtual float ProcessedValue () {
		return Value * Amplitude;
	}

	protected virtual void DecayValue () {

		Value = Value * Mathf.Exp(-DecayRate * Time.deltaTime);

	}

}
