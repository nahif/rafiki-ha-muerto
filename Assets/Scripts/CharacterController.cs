using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

	public float maxspeed=10f;
	public float deathChangeSpeed = 40f;
	public float actualspeed=0;
	private float lastYSpeed;
	public bool burried = false;
	bool facingRight=false;
	Animator anim;
	bool grounded;
	float groundedRadius = .2f;
	[SerializeField] LayerMask whatIsGround;
	Transform groundCheck;
	//Cambios comportamiento segun piso
	GroundCheckController gcc;



	void Start(){
		anim = GetComponent<Animator> ();
		gcc = (GroundCheckController) GameObject.Find("GroundCheck").GetComponent("GroundCheckController");
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Interact"),LayerMask.NameToLayer("Ignore Raycast"),true);
		//Esto ignora la colision del personaje (Que esta en IGnore Raycast) y las semillas
	}

	void  Awake() {
		groundCheck = transform.Find ("GroundCheck");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Mathf.Abs (rigidbody2D.velocity.y - lastYSpeed) >= deathChangeSpeed)
			Die ();
		lastYSpeed = rigidbody2D.velocity.y;
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
		//Movimiento horizontal
		if (burried) return;
		float move = Input.GetAxis ("Horizontal");
		string materialName = gcc.materialName;
		if (materialName == "Vacio") {
			rigidbody2D.velocity = new Vector2 (maxspeed * move, rigidbody2D.velocity.y);
		} else if (materialName == "Resbaladiso") {
			gcc.delay = Mathf.Lerp (gcc.delay, maxspeed * move, gcc.friction * gcc.friction * Time.deltaTime);
   			rigidbody2D.velocity = new Vector2 (gcc.delay, rigidbody2D.velocity.y);
 		} else {
			rigidbody2D.velocity = new Vector2 (maxspeed * move, rigidbody2D.velocity.y);
		}

		anim.SetFloat ("Speed",Mathf.Abs( maxspeed * move));
		anim.speed = Mathf.Abs (maxspeed * move)/5;

		if (move > 0 && !facingRight) {
			Flip();		
		}
		else if(move<0 && facingRight){
			Flip();
		}
		if(grounded) {
			int layermask=~((1<<LayerMask.NameToLayer("Interact")) | Physics2D.IgnoreRaycastLayer); //VA A IGNORAR INTERACT O PODEMOS PONER QUE SOLO PESQUE GROUND Y COLISION
			RaycastHit2D hit = Physics2D.Raycast (transform.position, -transform.up,Mathf.Infinity,layermask);
			if (hit.collider != null && hit.collider.tag!="Player") {
				//Debug.Log(Mathf.Atan2(hit.normal.y,hit.normal.x)*90/Mathf.PI);
				float angle=transform.eulerAngles.z;
				float angle2=Mathf.Atan2(hit.normal.y,hit.normal.x)*90/Mathf.PI-45;
				//if(angle<0)angle+=2*3.14f;
				//if(angle2<0)angle2+=2*3.14f;
				transform.rotation=Quaternion.AngleAxis(angle2,Vector3.forward);
			}

		} else {
			anim.SetFloat ("Speed",0);
			transform.rotation=Quaternion.AngleAxis(0, Vector3.up);
		}
	}
	
	void Update () {

		if (!burried &&(grounded) && Input.GetKeyDown (KeyCode.Space)) {
				rigidbody2D.AddForce (transform.up * 500);
		}
		if (gcc.materialName != "Plant" && !GetComponent<SeedPlanter>().planting && 
		    !burried && (grounded) && Input.GetKeyDown (KeyCode.DownArrow)) {
			Dig();
        }
		if (burried && Input.GetKeyDown(KeyCode.UpArrow)) {
			GoOut();
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

	void Dig() {
		burried = true;
		//CAMBIAR ANIMACION

	}

	void GoOut() {
		burried = false;
	}


}
