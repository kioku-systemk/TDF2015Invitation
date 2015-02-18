using UnityEngine;
using System.Collections;

public class KeyboardController : MonoBehaviour {

	void Start () {

	}

	void Update () {
		if (Input.GetKey("escape")) {
            Application.Quit();
		}
	}
}
