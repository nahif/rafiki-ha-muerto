using UnityEngine;
using System.Collections;

public class ExampleSeed : MonoBehaviour, ISeed {

	public Transform plant;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#region chequeador de suelos

	private bool puedeCrecer = false;
	private bool isPlanted=false;
	private Vector3 plantVector;
	public string debugS = "";

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.collider2D.sharedMaterial != null &&  coll.gameObject.collider2D.sharedMaterial.name.CompareTo("SueloFertil") == 0) {
			debugS = coll.gameObject.collider2D.sharedMaterial.name;
			puedeCrecer = true;
			/*&& coll.gameObject.collider2D.sharedMaterial.name == "SueloFertil" */
		} 
		else if (coll.gameObject.collider2D.sharedMaterial != null){
			puedeCrecer = false;		
		}
	}


	#endregion

	#region ISeed implementation

	public void grow ()
	{
		if (puedeCrecer & isPlanted) {
			Instantiate (plant, (transform.position+plantVector), Quaternion.identity);
			Destroy (this.gameObject);
		}
	}

	public void shrink ()
	{
		throw new System.NotImplementedException ();
	}

	public void shine ()
	{
		Debug.Log ("estoy brillando");
	}

	public void interact ()
	{
		throw new System.NotImplementedException ();
	}

	public void onPlant (Vector3 pox)
	{
		plantVector = pox;
		rigidbody2D.isKinematic = true;
		isPlanted = true;
	}

	public GameObject onCollect ()
	{
		if (isPlanted)
						return null;
			this.gameObject.active=false;
			return this.gameObject;
	}

	#endregion
}
