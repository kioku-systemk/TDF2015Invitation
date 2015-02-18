using UnityEngine;
using System.Collections;

public class KeyboardController : MonoBehaviour {

	void Start () {
		Screen.showCursor = false;
	}

	void Update () {
		if (Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
		}

		if (Input.GetKeyDown(KeyCode.R) ||
			Input.GetKeyDown(KeyCode.Space)) {
			if (OVRManager.display != null) {
				OVRManager.display.RecenterPose();
			}
		}
	}
}
