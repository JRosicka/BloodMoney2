using UnityEngine;

public class AnimatorLink : MonoBehaviour {

	public Animator LinkedAnimator;

	public void SetTrigger (string triggerString) {
		if (LinkedAnimator != null)
			LinkedAnimator.SetTrigger(triggerString);
	}

}
