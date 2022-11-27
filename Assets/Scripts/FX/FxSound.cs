//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

public class FxSound : FxPackage {

//	private List<AudioSource> sources = new List<AudioSource>();
//	private List<AudioClip> clips = new List<AudioClip>();
//	public SoundCategory category = SoundCategory.UI;
//	[Range(0F, 1F)] public float volume = 1F;
//	[Range(0F, 3F)] public float pitch = 1F;
//	[Range(0F, 1F)] public float pitchRandomness = 0.08F;
//	private int nextIdx = 0;
//
//	public override void SetUp (GameObject anchor = null) {
//		sources.Clear();
//		clips.Clear();
//		foreach (AudioSource source in GetComponents<AudioSource>()) {
//			sources.Add(source);
//			clips.Add(source.clip);
//		}
//	}
//
//
//	public override void Trigger (GameObject anchor = null) {
//		base.Trigger(anchor);
//		if (sources.Count > 0) {
//			int clipRandomizer = Random.Range(0, clips.Count);
//			SoundManager.PlaySound(clips[clipRandomizer], category, volume, false, pitch * Random.Range(1F - pitchRandomness, 1F + pitchRandomness), sources[clipRandomizer]);
//		}
//	}
//
//	public override void Toggle (bool toggleState, GameObject anchor = null) {
//		base.Toggle(toggleState, anchor);
//		if (toggleState) {
//			if (sources.Count > 0) {
//				int clipRandomizer = Random.Range(0, clips.Count);
//				SoundManager.PlaySound(clips[clipRandomizer], category, volume, false, pitch * Random.Range(1F - pitchRandomness, 1F + pitchRandomness), sources[clipRandomizer]);
//			}
//		}
//	}

}
