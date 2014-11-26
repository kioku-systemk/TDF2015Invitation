using UnityEngine;
using System.Collections;

public class DemoManager : MonoBehaviour {

	public GameObject[] scenePrefab;
	public float musicPartPerSecond; 
	public float[] sceneMusicPart;


	private GameObject[] sceneObject;
	void Awake()
	{
		// make scenes from prefabs
		sceneObject = new GameObject[scenePrefab.Length];
		
        for (var i = 0; i < scenePrefab.Length; i++) {
            sceneObject[i] = (GameObject)Instantiate(scenePrefab[i]);
            sceneObject[i].SetActive(false);
		}
        if (scenePrefab.Length > 0) {
	        sceneObject[0].SetActive(true); // ready first scene
	    }
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float tm = Time.time;
		for (int i = 0; i < sceneMusicPart.Length; ++i)
		{
			if (tm < sceneMusicPart[i] * musicPartPerSecond) {
				if (sceneObject[i].activeInHierarchy == false) {
					if (i > 0) {
						sceneObject[i - 1].SetActive(false);
					}
					sceneObject[i].SetActive(true);
				}
				break;
			}
		}
	}
}
