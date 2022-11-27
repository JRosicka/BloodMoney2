using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Simplifies usages of Flash-In Transitions
/// </summary>
[System.Serializable]
public class TimedEntranceTransition {
    
    public GameObject TransitionerPrefab;
    public GameObject TransitionedObject;
    public float Delay;
    public UnityEvent TransitionEvent;
    private float timer;

    public bool HasDoneTransition => hasDoneTransition;
    private bool hasDoneTransition;

    public void Initialize() {
        TransitionedObject.SetActive(false);
    }

    public void UpdateExternal() {
        if (!hasDoneTransition) {
            timer += Time.deltaTime;
            if (timer > Delay) {
                DoTransition();
            }
        }
    }

    public void DoTransition() {
        TransitionedObject.SetActive(true);
        Object.Instantiate(TransitionerPrefab, TransitionedObject.transform.position, Quaternion.identity, TransitionedObject.transform);
        TransitionEvent.Invoke();
        hasDoneTransition = true;
    }

}
