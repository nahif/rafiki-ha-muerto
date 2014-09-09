using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

	public float maxspeed=300f;
	public float actualspeed=0;
	bool facingRight=false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float move = Input.GetAxis ("Horizontal");
		rigidbody2D.velocity = new Vector2 (maxspeed * move, rigidbody2D.velocity.y);
		if (move > 0 && !facingRight) {
			Flip();		
		}
		else if(move<0 && facingRight){
			Flip();
		};
		RaycastHit2D hit = Physics2D.Raycast (transform.position, -transform.up);
		if (hit.collider != null && hit.collider.tag!="Player") {
			//Debug.Log(Mathf.Atan2(hit.normal.y,hit.normal.x)*90/Mathf.PI);
			transform.rotation=Quaternion.AngleAxis(Mathf.Atan2(hit.normal.y,hit.normal.x)*90/Mathf.PI-45,Vector3.forward);
		}
	}
	
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			rigidbody2D.AddForce(transform.up*500);
		}
	}
	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
