using UnityEngine;

/// <summary>
/// Changes the animation trigger state of an animator when triggered by an FxPackageController
/// </summary>
public class FxTriggerAnimator : FxPackage {

	//FACTOR ATTRIBUTES
	public string TriggerString = "Activate";

	//ANATOMY
	private Animator anim;

	public override void SetUp (GameObject newAnchor = null) {

		base.SetUp(newAnchor);
		if (newAnchor == null) return;
		Anchor = newAnchor;
		Debug.Log("Setting up SFXTriggerAnimator. Anchor = " + newAnchor);
		if (newAnchor.GetComponentInChildren<Animator>() != null)
			anim = newAnchor.GetComponentInChildren<Animator>();

	}

	public override void Trigger (GameObject newAnchor = null, float amplitude = 1f, float timeOffset = 0f) {
		base.Trigger(newAnchor, amplitude, timeOffset);
		if (anim == null) return;
		anim.SetTrigger(TriggerString);
		Debug.Log("Triggering Animator on object " + anim.gameObject.name);
	}

	public override void Toggle (bool toggleState, GameObject newAnchor = null, float amplitude = 1f, float timeOffset = 0f) {
		base.Toggle(toggleState, newAnchor, amplitude, timeOffset);
		if (anim != null && toggleState)
			anim.SetTrigger(TriggerString);
	}

}
