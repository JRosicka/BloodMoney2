using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rebar.Util {
	/// <summary>
	/// A set of utilities for manipulating Animation Curves.
	/// </summary>
	public static class AnimationCurveUtilities {
	
		public static AnimationCurve MultipliedCurve(AnimationCurve inputCurve, float amplitude) {
			if (Math.Abs(amplitude - 1f) < 0.001f) return inputCurve; 
			Keyframe[] frames = new Keyframe[inputCurve.keys.Length];
			for (int i = 0; i < inputCurve.keys.Length; i++) {
				frames[i] = new Keyframe(inputCurve.keys[i].time, inputCurve.keys[i].value * amplitude, inputCurve.keys[i].inTangent * amplitude, inputCurve.keys[i].outTangent * amplitude);
			}
			return new AnimationCurve(frames);
		}

		public static Keyframe FirstKey(this AnimationCurve curve) {
			return curve.keys[0];
		}

		public static Keyframe LastKey(this AnimationCurve curve) {
			return curve.keys[curve.keys.Length - 1];
		}

		/// <summary>
		/// Grabs the final "time" value among an AnimationCurve's keys. If there are no keys, 0f is returned.
		/// </summary>
		public static float Duration(this AnimationCurve curve) {
			return curve.keys.Length == 0 ? 0f : curve.LastKey().time;
		}
	
	}
}