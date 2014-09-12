using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

	public Transform Player;

	public Vector2 Margin, Smoothing;

	public BoxCollider2D Bounds;

	public Vector3 _min,_max;

	public bool IsFollowing { get; set; }

	// Use this for initialization
	void Start () {
		_min = Bounds.bounds.min;
		_max = Bounds.bounds.max;
	}
	
	// Update is called once per frame
	void Update () {
		var x = transform.position.x;
		var y = transform.position.y;
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
		y = Mathf.Lerp(y, Player.position.y,Smoothing.y * Time.deltaTime);

		//x = Player.position.x;
		//y = Player.position.y;



		transform.position = new Vector3 (x, y, transform.position.z);

	}

}
