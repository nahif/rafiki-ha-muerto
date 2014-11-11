using UnityEngine;
public interface ISeed {
	void grow();
	void shrink();
	void shine();
	void interact();
	void onPlant(Vector3 v);
	GameObject onCollect();
}
