using System;
using System.Collections.Generic;
using Rebar.Util;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Hooks onto an object with one or more graphics and causes those graphics to flicker in over about one second.
/// </summary>
public class FlashInTransitionController : MonoBehaviour {

	public Material WhiteMaterial;
	public AnimationCurve CanvasGroupAlphaCurve;
	private float TargetCanvasGroupAlpha => CanvasGroupAlphaCurve.Evaluate(Progress);
	public AnimationCurve FlashCurve;
	private Color TargetFlashColor => new Color(1f, 1f, 1f, FlashCurve.Evaluate(Progress));
	public AnimationCurve ScaleCurve;
	private Vector3 baseScale;
	private Vector3 TargetScale => baseScale * ScaleCurve.Evaluate(Progress);
	private Transform parentTransform;

	public bool FlashesIn;
	
	public float Duration = 1f;
	private float timer;
	private float Progress => timer / Duration;
	
	private CanvasGroup canvasGroup;
	private List<Graphic> graphics = new List<Graphic>();
	
	public interface IFlashesIn {
		List<Graphic> FlashInGraphics();
	}
	
	private void OnEnable() {
		timer = 0f;
		graphics.Clear();
		parentTransform = transform.parent;
		baseScale = parentTransform.localScale;
		canvasGroup = parentTransform.gameObject.AddComponent<CanvasGroup>();
		if (canvasGroup == null) return;
		canvasGroup.alpha = 0f;

		if (!FlashesIn) {
			return;
		}
		
		IFlashesIn flashesIn = parentTransform.GetComponent<IFlashesIn>();
		if (flashesIn != null) {
			foreach (Graphic g in flashesIn.FlashInGraphics()) {
				g.color = Color.white;
				g.material = WhiteMaterial;
				graphics.Add(g);
			}
		}
		else {
			foreach (Graphic g in parentTransform.GetComponentsInChildren<Graphic>()) {
				Transform gt = g.transform;
				if (g.GetComponent<Mask>() != null) continue;
				if (Math.Abs(g.color.a) > 0.001f) {
					// Ignore invisible graphics.
					GameObject newGameObject = Instantiate(g.gameObject, gt.position, gt.rotation, gt.parent);
					Graphic newGraphic = newGameObject.GetComponent<Graphic>();
					newGraphic.color = Color.white;
					newGraphic.material = WhiteMaterial;
					graphics.Add(newGraphic);
					foreach (Graphic childGraphic in newGameObject.transform.GetComponentsInChildren<Graphic>()) {
						if (childGraphic != newGraphic)
							Destroy(childGraphic.gameObject);
					}

					foreach (Transform t in newGameObject.transform) {
						Destroy(t.gameObject);
					}
				}
			}
		}

	}

	private void Update() {
		timer += Time.deltaTime;
		if (Progress >= 1f) {
			EndTransition();
			return;
		}

		if (canvasGroup == null) return;
		canvasGroup.alpha = TargetCanvasGroupAlpha;
		Color targetColor = TargetFlashColor;
		foreach (Graphic g in graphics) {
			g.color = targetColor;
		}
		parentTransform.localScale = TargetScale;
	}

	private void OnDisable() {
		EndTransition();
	}

	public void EndTransition() {
		Destroy(canvasGroup);
		foreach (Graphic g in graphics) {
			Destroy(g.gameObject);
		}
		if(parentTransform) parentTransform.localScale = baseScale;
		Destroy(gameObject);
	}
	
}
