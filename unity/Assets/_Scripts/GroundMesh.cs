using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Utility;
using HTC.UnityPlugin.Vive;
using HTC.UnityPlugin.PoseTracker;

public class GroundMesh : MonoBehaviour {

	public InputManager input;
	public GameObject[] activateFor;


	void Update(){
		if(input.S_Pad_Press)
			foreach(var active in activateFor){
				if (active.activeInHierarchy){
					GetComponent<MeshRenderer>().enabled = true;
					return;
				}
			}

		GetComponent<MeshRenderer>().enabled = false;
	}
}
