using System.Collections.Generic;
using Rebar.Util;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Blends colors and applies them to a graphic or sprite. Uses weighted colors, and blends them into the baseColor,
/// which is the On-Awake color of the target graphic with a strength of 1.
/// </summary>
public class FxApplierColor : FxApplier {
	private Color baseColor;
	
	private Graphic graphicTarget;
	private SpriteRenderer spriteTarget;
	private bool usesGraphic;
	private bool setup;

	private readonly List<ColorWeight> factorWeights = new List<ColorWeight>();
	private Color finalColor;

	public override void SetUp (GameObject newAnchor = null) {
		if (newAnchor == null)
			return;

		setup = true;
		
		graphicTarget = GetComponent<Graphic>();
		spriteTarget = GetComponent<SpriteRenderer>();

		usesGraphic = graphicTarget != null;
		baseColor = usesGraphic ? graphicTarget.color : spriteTarget.color;
	}

	public void AddColorFactor (int sfxId, float newValue, float newDecayRate, Color newColor, float timeOffset = 0f) {
		
		enabled = true;	// Enable this when it is interacted with.
		
		FxFactorColor factor = (FxFactorColor)FactorWithId(sfxId);
		if(factor != null) {
			factor.Value = newValue;
			factor.Color = newColor;
		} else {
			factor = new FxFactorColor {SfxId = sfxId, Value = newValue, DecayRate = newDecayRate, Color = newColor};
			factor.ApplyTimeOffset(timeOffset);
			ActiveFactors.Add(factor);
		}
	}

	private void Update () {
		ApplyColor();
		AgeFactors();

		if (ActiveFactors.Count == 0) {	// Disable this when it is no longer processing factors.
			enabled = false;
		}
	}

	private void ApplyColor () {
		if (!setup)
			return;
		
		if (ActiveFactors.Count == 0) {
			finalColor = baseColor;
		} else {
			factorWeights.Clear();
			factorWeights.Add(new ColorWeight(baseColor, 1f));
			foreach (FxFactor factor in ActiveFactors) {
				factorWeights.Add(new ColorWeight(((FxFactorColor) factor).Color, factor.Value));
			}

			finalColor = ColorUtilities.BlendColors(factorWeights);
		}

		if (usesGraphic) graphicTarget.color = finalColor;
		else spriteTarget.color = finalColor;
	}

	public void SetBaseColor(Color color) {
		baseColor = color;
	}

}

public class FxFactorColor : FxFactor {

	public Color Color;
	
}
