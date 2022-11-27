using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// Handles a common tweening situation where a value should blend between two different curves.
/// A very frequent example might be the alpha channel of an object that fades in and out. The controller
/// might use a SuperSmooth to set two curves (the way in which it fades in and out) and then set the
/// alpha every frame based on what SuperSmooth.UpdateValue returns.
///
/// A SuperSmooth should be used in place of ordinary tweening curves when momentum is important. For example,
/// if a sphere is at one location in its "active" state and at another position for its "inactive" state, then
/// quickly activating and deactivating the object should produce some "conservation of momentum", which is expressed
/// by the velocity in this script.
/// </summary>
[Serializable]
public class SuperSmooth {

    // Public Fields
    public AnimationCurve ActivateCurve;    // The curve as the value blends into its active state.
    public AnimationCurve DeactivateCurve;  // The curve as the value blends into its inactive state.
    public float EaseTime = 0.05f;          // Higher = more languid acceleration between curves. Above 0.15 almost always looks bad.

    // Private Fields
    // State Change
    private bool isActive;
    private float timeOfLastChange;
    private float prevValue = 0f;
    
    // Value Updating
    private float DestinationValue => isActive ? 1f : 0f;
    private AnimationCurve CurrentCurve => isActive ? ActivateCurve : DeactivateCurve;
    private float CurTargetValue => Mathf.LerpUnclamped(prevValue, DestinationValue, CurrentCurve.Evaluate(Time.time - timeOfLastChange));
    private float TimeSinceLastChange => Time.time - timeOfLastChange;
    private float curValue;
    private float valueVelocity;

    // Deactivation Action
    private Action actionOnFullDeactivate;

    /// <summary>
    /// Call this once per frame.
    /// </summary>
    /// <returns>The object's "progress" toward being fully active. Use this to tween between states.</returns>
    public float UpdateValue() {
        curValue = Mathf.SmoothDamp(curValue, CurTargetValue, ref valueVelocity, EaseTime);
        if (!isActive && actionOnFullDeactivate != null && TimeSinceLastChange > DeactivateCurve.keys.Last().time) {
            actionOnFullDeactivate.Invoke();
            actionOnFullDeactivate = null;
        }
        return curValue;
    }

    /// <summary>
    /// Get the current value without updating it.
    /// </summary>
    public float GetValue() {
        return curValue;
    }

    /// <summary>
    /// Set whether the object is in an Active state (uses the ActivateCurve) or Inactive state (uses the DeactivateCurve).
    /// </summary>
    /// <param name="shouldBeActive">Whether the object is active.</param>
    public void SetState(bool shouldBeActive) {
        if ((!isActive && shouldBeActive) || (isActive && !shouldBeActive)) {
            timeOfLastChange = Time.time;
        }
        isActive = shouldBeActive;
    }

    /// <summary>
    /// Directly sets the new value. Used in testing, and potentially a sign that a SuperSmooth may not be the best
    /// way to store and modify this value.
    /// </summary>
    public void SetValue(float newValue) {
        curValue = newValue;
    }

    /// <summary>
    /// Set what happens which this object reaches the end of the deactivation curve.
    /// </summary>
    /// <param name="action">The action that is performed.</param>
    public void SetActionOnFullDeactivate(Action action) {
        actionOnFullDeactivate = action;
    }

    /// <summary>
    /// Create a new SuperSmooth by copying another.
    /// </summary>
    public SuperSmooth(SuperSmooth other) {
        ActivateCurve = other.ActivateCurve;
        DeactivateCurve = other.DeactivateCurve;
        EaseTime = other.EaseTime;
    }

    /// <summary>
    /// Empty constructor for testing.
    /// </summary>
    public SuperSmooth() { }

    public override bool Equals(object other) {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        if (other.GetType() != GetType()) return false;
        return Equals((SuperSmooth)other);
    }

    public override int GetHashCode() {
        throw new NotImplementedException("this really shouldn't be used as an index...");
    }

    /// <summary>
    /// compare the values of two SuperSmooth objects. primarily used for testing.
    /// </summary>
    public bool Equals(SuperSmooth other) {
        if (other == null) return false;
        if (!ActivateCurve.Equals(other.ActivateCurve)) return false;
        if (!DeactivateCurve.Equals(other.DeactivateCurve)) return false;
        if (!Mathf.Approximately(EaseTime, other.EaseTime)) return false;
        return true;
    }
}
