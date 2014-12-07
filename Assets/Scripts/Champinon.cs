using UnityEngine;
using System.Collections;

public class Champinon : MonoBehaviour{
	// Use this for initialization
	Animator anim;
	bool wt=false;
	void Start () {
		gameObject.AddComponent<IPlant> ();
		anim = this.GetComponentInParent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (wt) {
			anim.SetBool ("Jump", false);
			wt=false;
				}
		if (anim.GetBool ("Jump"))
						wt = true;
	}

	void OnCollisionEnter2D(Collision2D other){

		anim.SetBool ("Jump", true);
		float nX = Mathf.Min (1.5f * other.gameObject.rigidbody2D.velocity.x, 20f);
		float nY = Mathf.Min (1.5f * other.gameObject.rigidbody2D.velocity.y, 20f);
		other.gameObject.rigidbody2D.velocity=new Vector2 (nX, nY);
	}
}
