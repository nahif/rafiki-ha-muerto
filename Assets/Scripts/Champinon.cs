using UnityEngine;
using System.Collections;

public class Champinon : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D other){
		float nX = Mathf.Min (1.5f * other.gameObject.rigidbody2D.velocity.x, 20f);
		float nY = Mathf.Min (1.5f * other.gameObject.rigidbody2D.velocity.y, 20f);
		other.gameObject.rigidbody2D.velocity=new Vector2 (nX, nY);
	}
}
