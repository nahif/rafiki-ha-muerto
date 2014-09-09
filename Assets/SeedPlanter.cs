using UnityEngine;
using System.Collections;

public class SeedPlanter : MonoBehaviour {

	// Use this for initialization
	public float radius = 10f;
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Collider2D[] Seeds = Physics2D.OverlapCircleAll (transform.position, radius);
		foreach (Collider2D CSeed in Seeds) {
			MonoBehaviour[] list = CSeed.gameObject.GetComponents<MonoBehaviour>();
			foreach(MonoBehaviour mb in list){
				if(mb is ISeed){
					ISeed seed= mb as ISeed;
					seed.shine();
				}
			}
		}
	}
}
