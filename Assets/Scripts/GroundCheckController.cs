using UnityEngine;
using System.Collections;

public class GroundCheckController : MonoBehaviour {

	//Almacen el nombre y friccion del material en el cual esta el personaje
	public string materialName;
	public float friction;
	public float delay;


	// Use this for initialization
	void Start () {
		materialName = "Vacio";
		friction = 1.0f;
		delay = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.sharedMaterial != null && materialName != other.sharedMaterial.name) {
			materialName = other.sharedMaterial.name;
			friction = other.sharedMaterial.friction;
			}
	}


	void OnTriggerExit2D(Collider2D other) {
		materialName = "Vacio";
		friction = 1.0f;
		delay = 0.0f;
	}
}
