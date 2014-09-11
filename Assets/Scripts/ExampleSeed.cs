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

	#region ISeed implementation

	public void grow ()
	{
		Instantiate( plant, transform.position, Quaternion.identity);
		Destroy (this.gameObject);
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
