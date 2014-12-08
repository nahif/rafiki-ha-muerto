using UnityEngine;
using System.Collections;

public class InfoScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = new Vector3(15, Screen.height - 15, 10);
		transform.position = Camera.main.ScreenToWorldPoint(pos);
	}
}
