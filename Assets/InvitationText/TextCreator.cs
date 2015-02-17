using UnityEngine;
using System.Collections;

public class TextCreator : MonoBehaviour {

	public static string[] texts = {
		"Are you interested in generative art?",
		"Computer graphics?",
		"Digital music?",
		"Interactive animation?",
		"Do you have plans this weekend?",

		"We invite you to",


		"21st, 22nd of February",
		"at the Institut Français du Japon",
		"Iidabashi, Tokyo",
		"Japan",
	};

	public GameObject textObject;
	public Color white = Color.white;
	public Color red = Color.red;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void ShowText(int i) {
		Debug.Log("Show text " + i);

		var instance = Instantiate(textObject) as GameObject;

		var textMesh = instance.GetComponent<TextMesh>();
		textMesh.text = texts[i];

		textMesh.color = (i < 5 ? white : red);
	}
}
