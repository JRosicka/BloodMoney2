using System;
using UnityEngine;

/// <summary>
/// Handles fading a canvas group in or out
/// </summary>
public class CanvasGroupFader : MonoBehaviour {
	public float Speed = 2f;
	public CanvasGroup Target;
	private int direction;
	private Action completeAction;

	public void AllIn() {
		direction = 0;
		Target.alpha = 1;
	}

	public void AllOut() {
		direction = 0;
		Target.alpha = 0;
	}

	public void FadeIn(Action whenComplete=null) {
		direction = 1;
		completeAction = whenComplete;
		enabled = true;
	}

	public void FadeOut(Action whenComplete=null) {
		direction = -1;
		completeAction = whenComplete;
		enabled = true;
	}

	private void Update() {
		if (direction == 0) return;
		float alpha = Target.alpha;
		alpha += Time.deltaTime * Speed * direction;
		Target.alpha = alpha;
		if (direction < 0 && alpha > 0) return;
		if (direction > 0 && alpha < 1) return;
		direction = 0;
		if (completeAction != null) completeAction();
		completeAction = null;
		enabled = false;
	}
}
