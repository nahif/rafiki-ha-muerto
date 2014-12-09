using UnityEngine;
using System.Collections;

public class NextLevelScript : MonoBehaviour {
	public GameObject mockle;
	public string nextLevel;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject == mockle) {
			Application.LoadLevel (nextLevel);
			Debug.Log ("Cargando Nivel:"+nextLevel);
		}
	}
}
