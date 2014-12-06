using UnityEngine;
using System.Collections;

public class SeedPlanter : MonoBehaviour
{

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
	private int[] seeds = new int[] {0, 0}; //0 = normal, 1 = hongo
	private int seedIndex = 0;

	public Camera camera;
	Animator mainAnim;

	CharacterController cController;

	void Start ()
	{
		aura = Resources.Load ("Prefabs/Aura") as GameObject;
		planterAura = (Instantiate (aura) as GameObject).transform;
		auraRenderer = planterAura.GetComponent<SpriteRenderer> ();
		auraRenderer.color = new Color (1f, 1f, 1f, 0.3f);
		auraRenderer.enabled = false;
		startingRendererSize = auraRenderer.bounds.size;
		maxAuraScale = new Vector2 (2f * radius / startingRendererSize.x, 2f * radius / startingRendererSize.y);
		cController = GetComponent<CharacterController> ();
		mainAnim = GetComponent<Animator> ();
	}

		// Update is called once per frame
		float t = 0f;

	void CollectSeed (bool isBurried)
	{
		Collider2D[] Seeds = Physics2D.OverlapCircleAll (transform.position, 1f);
		foreach (Collider2D CSeed in Seeds) {
			MonoBehaviour[] list = CSeed.gameObject.GetComponents<MonoBehaviour> ();
				foreach (MonoBehaviour mb in list) {
					/*if (mb is Seed) {
						Seed seed = mb as Seed;
						if (!isBurried && seed.isPlanted ()) {
							continue;
						}
						myCollectedObject = seed.onCollect ();
						break; //Solo queremos una. (SE PUEDE MEJORAR SELECCIONANDO LA MAS CERCANA)
					}
				}*/
				if (mb is Seed) {
					Seed seed = mb as Seed;
					if (seed is NormalSeed) {
						seeds[0]++;	
					} else if (seed is ChampiSeed) {
						seeds[1]++;
					}
					seed.onCollect();
					if (myCollectedObject != null) {
						break; //Por el doble Foreach
					}
				}
			}
		}
	}

	void Update ()
	{
		if (!GetComponent<CharacterController> ().burried) {
			//CODIGO PARA RECOGER UNA SEMILLA, TOMA LA PRIMERA Y BOTA LA QUE TIENE (SI TIENE)
			//Planteo:: Interfaz COLLECTABLE util? (Por si hay mas que semillas)

			CollectSeed (false);
		 /*else {
				myCollectedObject.active = true;
				myCollectedObject.GetComponent<Transform> ().position = transform.position;
				myCollectedObject = null;
				Debug.Log ("Se supone que tengo que soltar una semilla");
			}*/
			if (cController.gcc.materialName != "Plant" && !planting && 
			   	!cController.burried && cController.grounded && Input.GetKey (KeyCode.DownArrow)) {
				if (Input.GetKeyDown(KeyCode.Z)) {
                   	Dig();

					plantASeed();

					GoOut(); //Hay que animar esto
               	}
            }
			if (Input.GetKeyDown (KeyCode.C)) {
				seedIndex = (seedIndex + 1)%seeds.Length;
				Debug.Log("SELECCIONADO: " + seedIndex);
			}
			if (Input.GetKeyDown (KeyCode.X)) {
				t = 0f;				
				planting = true;
			}
			if (Input.GetKeyUp (KeyCode.X)) {
				t = 0f;
				planting = false;
				float currentRadius = planterAura.localScale.x * startingRendererSize.x / 2f;
				Collider2D[] Seeds = Physics2D.OverlapCircleAll (transform.position, currentRadius);
				foreach (Collider2D CSeed in Seeds) {
					MonoBehaviour[] list = CSeed.gameObject.GetComponents<MonoBehaviour> ();
					foreach (MonoBehaviour mb in list) {
						/*if (!Input.GetKey (KeyCode.DownArrow) && mb is Seed) {
							Seed seed = mb as Seed;
							seed.shine (); // Esto deberia estar en PLANTING por que la idea es que "Brille" al seleccionarlo.
							seed.grow ();
						}*/
						if (Input.GetKey (KeyCode.DownArrow) && mb is IPlant) {
							IPlant plant = mb as IPlant;
							plant.Sel ();
							GameObject newSeed = Instantiate (Resources.Load ("Prefabs/Semilla")) as GameObject;
							Transform newSeedTransform = newSeed.transform;
							Seed seed = null;
							if (plant.type == IPlant.Type.normal) {
								newSeed.AddComponent<NormalSeed>();
								seed = newSeed.GetComponent<NormalSeed> ();
							} else if (plant.type == IPlant.Type.champinon){
								newSeed.AddComponent<ChampiSeed>();
								seed = newSeed.GetComponent<ChampiSeed> ();
							}
							seed.wasPlant = true;
							seed.plant = plant.GetComponent<Transform> ();
							newSeedTransform.transform.position = plant.transform.position;
							plant.gameObject.SetActive (false);
							Debug.Log ("Vuelve planta a semilla");
							//CREAR UNA SEED Y agregarle la planta.
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
				if (myCollectedObject != null) {
					Seed seed = myCollectedObject.GetComponent<MonoBehaviour> () as Seed;
					myCollectedObject.active = true;
					myCollectedObject.GetComponent<Transform> ().position = transform.position - transform.up;
					myCollectedObject = null;
					seed.onPlant (transform.up);
					Debug.Log ("Se supone que enterre una semilla");
				}/* else {
										CollectSeed (true);
										Debug.Log ("Desenterrar");
								}*/
						
			}
		}

	void plantASeed() {
		if (seeds[seedIndex] > 0) {
			GameObject seed = null;
			switch(seedIndex) {
			case 0:
				seed = Resources.Load ("Prefabs/NormalSeed") as GameObject;
				break;
            case 1:
				seed = Resources.Load ("Prefabs/ChampiSeed") as GameObject;
                break;
            }
			GameObject realSeed = Instantiate(seed) as GameObject;
			Seed seedController = realSeed.GetComponent<MonoBehaviour> () as Seed;
			realSeed.active = true;

			realSeed.GetComponent<Transform> ().position = transform.position - transform.up;
			seeds[seedIndex]--;
			seedController.onPlant (transform.up);
			seedController.grow();
			Destroy(realSeed);

        } else {
			Debug.Log("No queda de este tipo de semillas");
		}
    }

	void Dig() {
		cController.rigidbody2D.isKinematic = true;
		mainAnim.SetBool ("Dig", true);

		//CAMBIAR ANIMACION
		
	}
	
	void GoOut() {
		rigidbody2D.isKinematic = false;
        mainAnim.SetBool ("Dig", false);
    }

}
