using UnityEngine;
using System.Collections;

public class IPlant : MonoBehaviour {
	public enum Type {
		normal,
		champinon
	}
	
	public Type type;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Sel(){
		Debug.Log ("sELECTeD");
	}

}
