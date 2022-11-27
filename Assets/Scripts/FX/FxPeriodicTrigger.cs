using UnityEngine;

public class FxPeriodicTrigger : MonoBehaviour {

	public FxPackageController FxController;
	[Range (0.01F, 15F)]public float Period = 1F;
	private float timer;

	void Update () {
		timer += Time.deltaTime;
		if (timer > Period) {
			timer = 0F;
			FxController.TriggerAll();
		}
	}

}
