using UnityEngine;
using System.Collections;

public class ExampleSeed : MonoBehaviour, ISeed {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#region ISeed implementation

	public void grow ()
	{
		throw new System.NotImplementedException ();
	}

	public void shrink ()
	{
		throw new System.NotImplementedException ();
	}

	public void shine ()
	{
		Debug.Log ("I Shine Like a Diamond");
		rigidbody2D.AddForce (Vector2.up*40);
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
