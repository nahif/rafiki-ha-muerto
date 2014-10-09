using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

	public float maxspeed=300f;
	public float deathSpeed = 20f;
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
				float angle=transform.eulerAngles.z;
				float angle2=Mathf.Atan2(hit.normal.y,hit.normal.x)*90/Mathf.PI-45;
				//if(angle<0)angle+=2*3.14f;
				//if(angle2<0)angle2+=2*3.14f;
				transform.rotation=Quaternion.AngleAxis(angle,Vector3.forward);
			}
			if (rigidbody2D.velocity.y <= -deathSpeed) {
				Die ();
				Debug.Log("DIEEEEEEEEEEEEEEEE");
			}
		} else {
			transform.rotation=Quaternion.AngleAxis(0, Vector3.up);
		}
	}
	
	void Update () {
		if (grounded && Input.GetKeyDown (KeyCode.Space)) {
			rigidbody2D.AddForce(transform.up*500);
		}
	}
	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void Die() {
		Transform respawner = GameObject.FindGameObjectWithTag ("Respawn").transform;
		transform.position = respawner.position;
	}
}
