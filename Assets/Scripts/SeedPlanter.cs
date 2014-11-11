using UnityEngine;
using System.Collections;

public class SeedPlanter : MonoBehaviour {

	// Use this for initialization
	public GameObject aura;
	public Transform planterAura;
	public SpriteRenderer auraRenderer;
	public float radius = 5f;
	private Vector2 maxAuraScale;
	public bool planting = false;
	public float auraGrowTime = 0.5f;
	public float auraShrinkTime = 0.3f;
	private Vector2 startingRendererSize;
	private GameObject myCollectedObject; //Aqui se guarda lo que hayamos recogido.

	public Camera camera;

	void Start () {
		aura = Resources.Load ("Prefabs/Aura") as GameObject;
		planterAura = (Instantiate(aura) as GameObject).transform;
		auraRenderer = planterAura.GetComponent<SpriteRenderer> ();
		auraRenderer.color = new Color(1f,1f,1f,0.3f);
		auraRenderer.enabled = false;
		startingRendererSize = auraRenderer.bounds.size;
		maxAuraScale = new Vector2 ( 2f *radius / startingRendererSize.x, 2f *radius / startingRendererSize.y);
	}

	// Update is called once per frame
	float t = 0f;
	void Update () {


		if (!GetComponent<CharacterController> ().burried) {

						if (Input.GetKeyDown (KeyCode.X)) {
								//CODIGO PARA RECOGER UNA SEMILLA, TOMA LA PRIMERA Y BOTA LA QUE TIENE (SI TIENE)
								//Planteo:: Interfaz COLLECTABLE util? (Por si hay mas que semillas)
								if (myCollectedObject == null) {
										Debug.Log ("Se supone que deberia atrapar una semilla");
										Collider2D[] Seeds = Physics2D.OverlapCircleAll (transform.position, 5f);
										foreach (Collider2D CSeed in Seeds) {
												MonoBehaviour[] list = CSeed.gameObject.GetComponents<MonoBehaviour> ();
												foreach (MonoBehaviour mb in list) {
														if (mb is ISeed) {
																ISeed seed = mb as ISeed;
																myCollectedObject = seed.onCollect ();
																if(myCollectedObject==null)continue;
																break; //Solo queremos una. (SE PUEDE MEJORAR SELECCIONANDO LA MAS CERCANA)
														}
												}
												if (myCollectedObject != null)
														break; //Por el doble Foreach
										}
								} else {
										myCollectedObject.active = true;
										myCollectedObject.GetComponent<Transform> ().position = transform.position;
										myCollectedObject = null;
										Debug.Log ("Se supone que tengo que soltar una semilla");
								}
						}

						if (Input.GetKeyDown (KeyCode.Z)) {
								t = 0f;
								planting = true;
						}
						if (Input.GetKeyUp (KeyCode.Z)) {
								t = 0f;
								planting = false;
								float currentRadius = planterAura.localScale.x * startingRendererSize.x / 2f;
								Collider2D[] Seeds = Physics2D.OverlapCircleAll (transform.position, currentRadius);
								foreach (Collider2D CSeed in Seeds) {
										MonoBehaviour[] list = CSeed.gameObject.GetComponents<MonoBehaviour> ();
										foreach (MonoBehaviour mb in list) {
												if (mb is ISeed) {
														ISeed seed = mb as ISeed;
														seed.shine (); // Esto deberia estar en PLANTING por que la idea es que "Brille" al seleccionarlo.
														seed.grow ();
												}
										}
								}
						}
						if (planting) {
								auraRenderer.enabled = true;
								planterAura.localScale = Vector2.Lerp (new Vector2 (0.1f, 0.1f), maxAuraScale, t / auraGrowTime);
								t += Time.deltaTime;
								camera.orthographicSize = Mathf.Lerp (camera.orthographicSize, 5.5f, Time.deltaTime * 0.3f);


						} else {
								planterAura.localScale = Vector2.Lerp (planterAura.localScale, new Vector2 (0.1f, 0.1f), t / auraShrinkTime);
								t += Time.deltaTime;
								camera.orthographicSize = Mathf.Lerp (camera.orthographicSize, 7.22f, Time.deltaTime * 1.2f);
								if (t / auraShrinkTime >= 1f) {
										auraRenderer.enabled = false;
								}
						}
						planterAura.position = transform.position;
				} else {

				if(Input.GetKeyDown (KeyCode.X) && myCollectedObject!=null){
					ISeed seed=myCollectedObject.GetComponent<MonoBehaviour>() as ISeed;
					myCollectedObject.active = true;
					myCollectedObject.GetComponent<Transform> ().position = transform.position - transform.up;
					myCollectedObject = null;
					seed.onPlant(transform.up);
					Debug.Log ("Se supone que enterre una semilla");
				}
				}
	}

}
