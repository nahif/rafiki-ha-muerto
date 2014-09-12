using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

	public float maxspeed=300f;
	public float actualspeed=0;
	bool facingRight=false;

	bool grounded;
	float groundedRadius = .2f;
	[SerializeField] LayerMask whatIsGround;
	Transform groundCheck;

	void  Awake() {
		groundCheck = transform.Find ("GroundCheck");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
		float move = Input.GetAxis ("Horizontal");
		rigidbody2D.velocity = new Vector2 (maxspeed * move, rigidbody2D.velocity.y);
		if (move > 0 && !facingRight) {
			Flip();		
		}
		else if(move<0 && facingRight){
			Flip();
		};
		if(grounded) {
			RaycastHit2D hit = Physics2D.Raycast (transform.position, -transform.up);
			if (hit.collider != null && hit.collider.tag!="Player") {
				//Debug.Log(Mathf.Atan2(hit.normal.y,hit.normal.x)*90/Mathf.PI);
				transform.rotation=Quaternion.AngleAxis(Mathf.Atan2(hit.normal.y,hit.normal.x)*90/Mathf.PI-45,Vector3.forward);
			}
		} else {
			transform.rotation=Quaternion.AngleAxis(0, Vector3.up);
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
