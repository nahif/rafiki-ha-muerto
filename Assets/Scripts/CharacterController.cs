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
	Animator Mainanim;
	bool grounded;
	float groundedRadius = .2f;
	[SerializeField] LayerMask whatIsGround;
	[SerializeField] LayerMask whatIsPlatform;
	Transform groundCheck;
	Transform headCheck;
	//Cambios comportamiento segun piso
	GroundCheckController gcc;



	void Start(){
		Mainanim = GetComponent<Animator> ();
		anim = GameObject.Find("cuerpo").GetComponent<Animator> ();
		gcc = (GroundCheckController) GameObject.Find("GroundCheck").GetComponent("GroundCheckController");
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Interact"),LayerMask.NameToLayer("Ignore Raycast"),true);
		//Esto ignora la colision del personaje (Que esta en IGnore Raycast) y las semillas
	}

	void  Awake() {
		groundCheck = transform.Find ("GroundCheck");
		headCheck = transform.Find ("HeadCheck");
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
		RaycastHit2D obj_below = Physics2D.Raycast (groundCheck.position, -Vector2.up, 1f, whatIsPlatform);
		if(obj_below.transform!=null && obj_below.collider.transform.position.y + obj_below.collider.bounds.size.y / 2 < groundCheck.position.y){ 
			obj_below.collider.isTrigger=false; 
		}
		
		RaycastHit2D obj_up = Physics2D.Raycast (headCheck.position, Vector2.up, 1f, whatIsPlatform);
		if(obj_up.transform!=null){    
			obj_up.collider.isTrigger=true;    
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
		rigidbody2D.isKinematic = true;
		Mainanim.SetBool ("Dig", true);
		//CAMBIAR ANIMACION

	}

	void GoOut() {
		burried = false;
		rigidbody2D.isKinematic = false;
		Mainanim.SetBool ("Dig", false);
	}


}
