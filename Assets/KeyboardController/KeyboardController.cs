using UnityEngine;
using System.Collections;

public class KeyboardController : MonoBehaviour {

	void Start () {
		Screen.showCursor = false;
	}

	void Update () {
		if (Input.GetKey("escape")) {
            Application.Quit();
		}
	}
}
