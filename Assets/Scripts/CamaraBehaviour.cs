using UnityEngine;
using System.Collections;

public class CamaraBehaviour : MonoBehaviour {

	public Transform Player;
	private Transform info;
	
	public Vector2 Margin, Smoothing;
	
	//public BoxCollider2D Bounds;
	
	//public Vector3 _min,_max;
	
	public bool IsFollowing { get; set; }
	
	// Use this for initialization
	void Start () {
		info = GameObject.Find ("InfoSemilla").transform;
}
	
	// Update is called once per frame
	void Update () {
		 float x = transform.position.x;
		float y = transform.position.y;
		/*if (IsFollowing) {
			Debug.Log ("hola");
			if(Mathf.Abs(x - Player.position.x) > Margin.x)
			   x = Mathf.Lerp(x, Player.position.x,Smoothing.x * Time.deltaTime);
			if(Mathf.Abs(y -Player.position.y) > Margin.y)
			   y = Mathf.Lerp(y, Player.position.y,Smoothing.y * Time.deltaTime);
		}

		var cameraHalfWidth = camera.orthographicSize * ((float)Screen.width / Screen.height);

		x = Mathf.Clamp (x, _min.x + cameraHalfWidth, _max.x - cameraHalfWidth);
		y = Mathf.Clamp (y, _min.y + camera.orthographicSize, _max.y - camera.orthographicSize);
		*/
		
		x = Mathf.Lerp(x, Player.position.x,Smoothing.x * Time.deltaTime);
		y = Mathf.Lerp (y, Player.position.y+2, Smoothing.y * Time.deltaTime);
		//float x = Player.position.x;
		//float y = Player.position.y+3;
		
		
		
		transform.position = new Vector3 (x, y, transform.position.z);
		Vector3 pos = new Vector3(20, Screen.height - 20, 10);
		info.position = Camera.main.ScreenToWorldPoint(pos);
	}
}
