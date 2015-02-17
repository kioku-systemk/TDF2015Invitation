using UnityEngine;
using System.Collections;

public class TextController : MonoBehaviour {
	void Start () {
	}

	void Update () {
	}

	public void AnimationFinished() {
		Destroy(gameObject);
	}
}
