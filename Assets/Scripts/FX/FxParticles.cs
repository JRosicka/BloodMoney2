using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Grabs particles attached to this object and calls Play, Emit, or Stop when the controlling FxPackageController
/// is triggered or toggled.
/// </summary>
public class FxParticles : FxPackage {

	//ATTRIBUTES
	public bool RendersUnderAnchor = true;
	public bool AddsCanvas = true;
	public bool SimpleEmit = true;
	public int SortingOrder = 5000;

	//ANATOMY
	private readonly List<ParticleSystem> systemList = new List<ParticleSystem>();

	public override void SetUp (GameObject newAnchor = null) {

		base.SetUp(newAnchor);
		if (newAnchor == null) return;
		Anchor = newAnchor;
		systemList.Clear();
		foreach (ParticleSystem system in GetComponentsInChildren<ParticleSystem>()) {
			systemList.Add(system);
		}

		//Add canvas component to parent, pushing it to a higher rendering order.
		if (!RendersUnderAnchor) return;
		if (AddsCanvas && newAnchor.GetComponent<Canvas>() == null) {
			newAnchor.AddComponent<Canvas>();
			newAnchor.GetComponent<Canvas>().overrideSorting = true;
			newAnchor.GetComponent<Canvas>().sortingOrder = SortingOrder;
		}

	}

	public override void Trigger (GameObject newAnchor = null, float amplitude = 1f, float timeOffset = 0f) {
		base.Trigger(newAnchor, amplitude, timeOffset);
		if (SimpleEmit) {
			foreach (var t in systemList) {
				t.Emit((int) (t.emission.rateOverTime.Evaluate(0f) * amplitude));
			}
		} else {
			foreach (var t in systemList) {
				t.Play();
			}
		}
	}

	public override void Toggle (bool toggleState, GameObject newAnchor = null, float amplitude = 1f, float timeOffset = 0f) {
		if (ToggleState != toggleState) {
			MultiplyEmission(amplitude / LastSetAmplitude);
			base.Toggle(toggleState, newAnchor, amplitude, timeOffset);
			foreach (var t in systemList) {
				if (toggleState)
					t.Play();
				else
					t.Stop();
			}
		}
	}

	private void MultiplyEmission(float ratio) {
		foreach (var t in systemList) {
			ParticleSystem.EmissionModule emission = t.emission;
			ParticleSystem.MinMaxCurve rateOverTime = emission.rateOverTime;
			rateOverTime.constantMin *= ratio;
			rateOverTime.constantMax *= ratio;
			rateOverTime.curveMultiplier *= ratio;
			for (int i = 0; i < emission.burstCount; i++) {
				ParticleSystem.Burst b = emission.GetBurst(i);
				ParticleSystem.MinMaxCurve bCurve = b.count;
				bCurve.constantMin *= ratio;
				bCurve.constantMax *= ratio;
				bCurve.curveMultiplier *= ratio;
			}
		}
	}

}
