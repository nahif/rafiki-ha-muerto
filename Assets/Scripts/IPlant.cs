using UnityEngine;
using System.Collections;

public class IPlant : MonoBehaviour {
	public enum Type {
		normal,
		champinon
	}
	
	public Type type;





	public void Sel(){
		Debug.Log ("sELECTeD");
	}

}
