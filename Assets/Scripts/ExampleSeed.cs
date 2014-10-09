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

	public bool puedeCrecer = false;
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
		if (puedeCrecer) {
			Instantiate (plant, transform.position, Quaternion.identity);
			Destroy (this.gameObject);
		}
	}

	public void shrink ()
	{
		throw new System.NotImplementedException ();
	}

	public void shine ()
	{
		Debug.Log ("I Shine Like a Diamond");
	}

	public void interact ()
	{
		throw new System.NotImplementedException ();
	}

	public void onPlant ()
	{
		throw new System.NotImplementedException ();
	}

	public void onCollect ()
	{
		throw new System.NotImplementedException ();
	}

	#endregion
}
