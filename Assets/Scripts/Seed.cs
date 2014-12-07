using UnityEngine;
public class Seed : MonoBehaviour {
	public Transform plant;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	#region chequeador de suelos
	
	private bool puedeCrecer = false;
	private bool Planted=false;
	private Vector3 plantVector;
	public string debugS = "";
	public bool wasPlant=false;
	
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
	
	#region metodos
	
	public void grow ()
	{
		//if (puedeCrecer & Planted) {
		if (Planted) {
			Debug.Log("HOLA");
			if(wasPlant==false){
				Instantiate (plant, (transform.position+plantVector), Quaternion.identity);
			}
			else{
				plant.gameObject.SetActive(true);
				plant.transform.position=transform.position+plantVector;
			}
			//Destroy (this.gameObject); AHORA SE HACE EN SEEDPLANTER
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
		Planted = true;
	}
	
	public GameObject onCollect ()
	{
		GameObject particulas = Resources.Load ("Prefabs/GrabSeedEx") as GameObject;
		Transform particulasIns = (Instantiate (particulas) as GameObject).transform;
		particulasIns.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,10);
		this.gameObject.SetActive (false);
		Planted = false;
		rigidbody2D.isKinematic = false;
		return this.gameObject;
	}
	public bool isPlanted(){
		return Planted;
	}

	
	#endregion

}
