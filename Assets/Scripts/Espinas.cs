using UnityEngine;
using System.Collections;

public class Espinas : MonoBehaviour {
	public Transform spawner;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		other.gameObject.transform.position = spawner.transform.position;
	}
}
