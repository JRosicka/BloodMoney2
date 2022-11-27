using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Synchronizes triggering or toggling a package of FX components. Can also be used to control a "droppable"
/// effects package that performs its function upon instantiation, then cleans itself up after finishing.
/// </summary>
public class FxPackageController : FxPackage {

	[Header("Anatomy")]
	public GameObject PackageAnchor;

	[Header("Auto-Play")]
	public bool TriggerOnStart;
	public bool ToggleOnStart;

	[Header("Toggle Settings")]
	public bool AutoOff;
	[Range(0F, 20F)] public float AutoOffTime = 1F;
	public bool AutoKill;
	[Range(0F, 20F)] public float AutoKillTime = 4F;
	[HideInInspector] public float Timer;
	private float timeScale = 1F;

	[Header("Test Buttons")]
	public bool TriggerTest;
	public bool ToggleTest;
	[Range(0f, 3f)] public float TestAmplitude = 1f;
	private float lastTestAmplitude;
	public bool RefreshSetupTest;

	private readonly List<FxPackage> fxList = new List<FxPackage> ();
	
	// ReSharper disable once InconsistentNaming, because this captures deltaTime.
	[HideInInspector] public float dT;

	private void Start () {
		
		if (!PackageAnchor && transform.parent)
			PackageAnchor = transform.parent.gameObject;

		foreach (FxPackage sfx in GetComponents<FxPackage>()) {
			sfx.Controller = this;
			if (sfx == this) continue;
			fxList.Add(sfx);
			sfx.SetUp(PackageAnchor);
		}

		if (TriggerOnStart)
			Trigger();
		if (ToggleOnStart)
			Toggle(true);

		lastTestAmplitude = TestAmplitude;

	}

	protected override void Update () {
		
		dT = Time.deltaTime * timeScale;	//Setting this here so constituent SFX components don't have to calculate it independently.

		if (ToggleState) {
			Timer += dT;
			if (AutoOff && Timer > AutoOffTime)
				ToggleAll(false);
			if (AutoKill && Timer > AutoKillTime)
				Destroy(gameObject);
		}

		//Test Features
		if (TriggerTest) {
			TriggerTest = false;
			TriggerAll(TestAmplitude);
		}

		if (ToggleTest) {
			ToggleTest = false;
			ToggleAll(!ToggleState, amplitude:TestAmplitude);
		}
		else if (Mathf.Abs(TestAmplitude - lastTestAmplitude) > 0.01f) {
			ToggleAll(ToggleState, amplitude:TestAmplitude);
		}

		if (RefreshSetupTest) {
			RefreshSetup();
		}
		
		#if !UNITY_EDITOR
		// Disable this if it isn't doing anything.
		if (!ToggleState) {
			enabled = false;
		}
		#endif

	}

	public void SetToggleState(bool toggleState) {
		ToggleAll(toggleState, PackageAnchor);
	}
	
	public void ToggleAll (bool toggleState, GameObject newAnchor = null, float amplitude = 1f, float timeOffset = 0f) {

		// Enable this when it is interacted with.
		enabled = true;
		
		if (!ToggleState && toggleState) {	
			Timer = 0F;	//Reset timer if appropriate
		}
		ToggleState = toggleState;
		if (newAnchor)
			PackageAnchor = newAnchor;
		foreach (FxPackage sfx in fxList)
			sfx.Toggle(toggleState, Anchor, amplitude, timeOffset);
		
	}

	public void TriggerAll (GameObject newAnchor, float amplitude = 1f, float timeOffset = 0f) {
		if (!this) return;
		// Enable this when it is interacted with.
		enabled = true;
		
		if (newAnchor)
			PackageAnchor = newAnchor;
		foreach (FxPackage sfx in fxList)
			sfx.Trigger(Anchor, amplitude, timeOffset);

	}

	// ReSharper disable once MethodOverloadWithOptionalParameter		this is fine.
	public void TriggerAll(float amplitude = 1f, float timeOffset = 0f) {
		TriggerAll(null, amplitude, timeOffset);
	}

	public void TriggerAll() {
		TriggerAll(null);
	}

	public void SetTimeScale (float newTimeScale) {
		
		// Enable this when it is interacted with.
		enabled = true;
		
		timeScale = newTimeScale;
	}

	public void RefreshSetup () {
		fxList.Clear();
		foreach (FxPackage sfx in GetComponents<FxPackage>()) {
			sfx.Controller = this;
			if (sfx == this) continue;
			fxList.Add(sfx);
			sfx.SetUp(PackageAnchor);
		}
	}

}
