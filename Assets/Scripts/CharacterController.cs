using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {
	
	public float maxspeed=5f;
	public float deathChangeSpeed = 40f;
	public float actualspeed=0;
	private float lastYSpeed;
	public bool burried = false;
	bool facingRight=false;
	public bool grounded;
	float groundedRadius = .2f;
	[SerializeField] LayerMask whatIsGround;
	[SerializeField] LayerMask whatIsPlatform;
	Transform groundCheck;
	Transform headCheck;
	//Cambios comportamiento segun piso
	public GroundCheckController gcc;

	//Animaciones:
	Animator anim;
	Animator Mainanim;
	
	
	
	void Start(){
		Mainanim = GetComponent<Animator> ();
		anim = GameObject.Find("MockleFinal").GetComponent<Animator>();
		gcc = (GroundCheckController) GameObject.Find("GroundCheck").GetComponent("GroundCheckController");
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Interact"),LayerMask.NameToLayer("Ignore Raycast"),true);
		//Esto ignora la colision del personaje (Que esta en IGnore Raycast) y las semillas

	}
	
	void  Awake() {
		groundCheck = transform.Find ("GroundCheck");
		headCheck = transform.Find ("HeadCheck");

	}
	
	float t = 0f;
	float lastAngle = 0f;
	void FixedUpdate () {
				if (Mathf.Abs (rigidbody2D.velocity.y - lastYSpeed) >= deathChangeSpeed)
						Die ();
				lastYSpeed = rigidbody2D.velocity.y;
				if (Physics2D.OverlapCircle (groundCheck.position, groundedRadius, whatIsGround)) {
						if (!grounded) {
								grounded = true;
						}
						t = 0f;
				} else {
						if (grounded)
								t = 0f;
						grounded = false;
				}
				//Movimiento horizontal
				if (burried)
						return;
				float move = Input.GetAxis ("Horizontal");
				
				if (move != 0){ 
							anim.SetFloat("Speed", Mathf.Abs (maxspeed * move));
			            	anim.speed = Mathf.Abs(maxspeed*move) / 5;
				}
				else{
							anim.SetFloat("Speed", 0);
					
				}
		
				string materialName = gcc.materialName;
				if (materialName == "Vacio") {
						rigidbody2D.velocity = new Vector2 (maxspeed * move, rigidbody2D.velocity.y);
				} else if (materialName == "Resbaladiso") {
						gcc.delay = Mathf.Lerp (gcc.delay, maxspeed * move, gcc.friction * gcc.friction * Time.deltaTime);
						rigidbody2D.velocity = new Vector2 (gcc.delay, rigidbody2D.velocity.y);
				} else {
						rigidbody2D.velocity = new Vector2 (maxspeed * move, rigidbody2D.velocity.y);
				}
		
				//anim.SetFloat ("Speed", Mathf.Abs (maxspeed * move));
				//anim.speed = Mathf.Abs (maxspeed * move) / 5;
		
				if (move > 0 && !facingRight) {
						Flip ();		
				} else if (move < 0 && facingRight) {
						Flip ();
				}
				if (grounded) {
						anim.SetBool("inGround",true);
						int layermask = ~((1 << LayerMask.NameToLayer ("Interact")) | Physics2D.IgnoreRaycastLayer); //VA A IGNORAR INTERACT O PODEMOS PONER QUE SOLO PESQUE GROUND Y COLISION
						RaycastHit2D hit = Physics2D.Raycast (groundCheck.position, -transform.up, 1f, layermask);
						if (hit.collider != null && hit.collider.tag != "Player") {
								//Debug.Log(Mathf.Atan2(hit.normal.y,hit.normal.x)*90/Mathf.PI);
								float angle = transform.eulerAngles.z;
								float angle2 = Mathf.Atan2 (hit.normal.y, hit.normal.x) * 90 / Mathf.PI - 45;
								if (lastAngle != angle2) { //Cambio de angulo
										t += Time.deltaTime;
										Quaternion quat = Quaternion.Lerp (transform.rotation, Quaternion.AngleAxis (angle2, Vector3.forward), t / 0.2f);
										float newAngle = quat.eulerAngles.z;
										transform.rotation = quat;
										lastAngle = newAngle;
								}
				
								//if(angle<0)angle+=2*3.14f;
								//if(angle2<0)angle2+=2*3.14f;
								//transform.rotation=Quaternion.AngleAxis(angle2,Vector3.forward);
						}
			
				} else {
			
						t += Time.deltaTime;
						//anim.SetFloat ("Speed", 1);
						transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.AngleAxis (0, Vector3.up), t / 2f);
				}
				RaycastHit2D obj_below = Physics2D.Raycast (groundCheck.position, -Vector2.up, 5f, whatIsPlatform);
				RaycastHit2D obj_between = Physics2D.Raycast (headCheck.position, -Vector2.up, 2f, whatIsPlatform);		
				RaycastHit2D obj_up = Physics2D.Raycast (headCheck.position, Vector2.up, 1f, whatIsPlatform);
				//bool colBelow = (obj_below.collider.transform.position.y + obj_below.collider.bounds.size.y / 2 < groundCheck.position.y);
		
				if (obj_up.transform != null) {    
						obj_up.collider.isTrigger = true;    
				}
				if (obj_between.transform != null) {    
						obj_between.collider.isTrigger = true;    
				}
		if (obj_below.transform != null ) { 
						obj_below.collider.isTrigger = false; 
				}
		}
	
	void Update () {
		
		if (!burried &&(grounded) && Input.GetKeyDown (KeyCode.Space)) {
			rigidbody2D.AddForce (transform.up * 500);
			anim.SetBool("isJumping",true);
		}
		/*if (gcc.materialName != "Plant" && !GetComponent<SeedPlanter>().planting && 
		    !burried && (grounded) && Input.GetKey (KeyCode.DownArrow)) {
			if (Input.GetKeyDown(KeyCode.Z)) {
				Dig();
			}
		}*/
		/*if (burried && Input.GetKeyDown(KeyCode.UpArrow)) {
			GoOut();
		}*/
		if (rigidbody2D.velocity.y <= 0) {
			headCheck.GetComponent<CircleCollider2D>().isTrigger = true;
			anim.SetBool("isFalling",true);
			anim.SetBool("isJumping",false);

		} else {
			headCheck.GetComponent<CircleCollider2D>().isTrigger = false;
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
	
	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "switch") {
			Debug.Log ("Switch");
			MonoBehaviour[] list = coll.gameObject.GetComponents<MonoBehaviour> ();
			foreach (MonoBehaviour mb in list) {
				if (mb is Switch && !(mb as Switch).isSwitched) {
					(mb as Switch).switchThisMotherfucker();
				}
			}
		}
	}
	
	
}
