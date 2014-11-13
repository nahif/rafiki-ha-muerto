using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Switch : MonoBehaviour {

	public bool isSwitched;
	public Transform[] colliders;
	public bool[] colliderEnable;
	public Transform[] switches;
	public bool[] switchesSwitch;
	// Use this for initialization
	void Start () {
	
	}

	public void switchThisMotherfucker() {
		for (int i = 0; i < colliders.Length; i++) {
			colliders[i].GetComponent<BoxCollider2D>().isTrigger = !colliderEnable[i];
		}
		for (int j = 0; j < switches.Length; j++) {
			MonoBehaviour[] list = switches[j].GetComponents<MonoBehaviour> ();
			foreach (MonoBehaviour mb in list) {
				if (mb is Switch) {
					if (switchesSwitch[j] == true) (mb as Switch).switchThisMotherfucker();
					else (mb as Switch).unSwitch();
				}
			}
		}
		isSwitched = true;
	}

	public void unSwitch() {
		for (int i = 0; i < colliders.Length; i++) {
			colliders[i].GetComponent<BoxCollider2D>().isTrigger = colliderEnable[i];
		}
		isSwitched = false;
	}


}
