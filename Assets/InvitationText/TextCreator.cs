using UnityEngine;
using System.Collections;

public class TextCreator : MonoBehaviour {

	public static string[] texts = {
		"Are you interested in generative art?",
		"Do you like computer graphics?",
		"Digital music makes you vibrate?",
		"Are you a creative mind?",
		"Do you have plans this weekend?",
		"We invite you to",
		"TOKYO DEMO FEST",
		"2015",
		"21st, 22nd of February",
		"at the Institut Francais du Japon",
		"Iidabashi, Tokyo",
		"Japan",
		"Seminars",
		"Oculus workshop",
		"Graphics competition",
		"GLSL competition",
		"Music competition",
		"Wild competition",
		"Demo competition",

		"Food and drinks",
		"Cozy party place",
		"Live DJ performance",
		"Lots of friends",
		"Lots of fun",
		"And much",
		"much",
		"more",
	};

	public GameObject textObject;
	public GameObject torusTextObject;
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

		if (i < 5) {
			var instance = Instantiate(textObject) as GameObject;
			var textMesh = instance.GetComponent<TextMesh>();
			textMesh.color = white;
			textMesh.text = texts[i];
		} else {
			var instance = Instantiate(torusTextObject) as GameObject;
			var textMesh = instance.GetComponent<TextMesh>();

			if (i == 6 || i == 7) {
				textMesh.characterSize = 2.5f;
			}
			textMesh.color = red;
			textMesh.text = texts[i];
		}
	}

	private bool IsOculusMode()
	{
        var oculus = GameObject.Find("OVRCamera");
		return (oculus != null && oculus.activeInHierarchy == true);
	}

	void EndDemo() {
		if (IsOculusMode() == false) {
			var debugFont = new GUIStyle();
			debugFont.fontSize = 24;
			debugFont.normal.textColor = Color.yellow;
			GUI.Label(new Rect(100f, 100f, 200f, 100f), "BYE", debugFont);
			Application.Quit();
		}
	}
}
