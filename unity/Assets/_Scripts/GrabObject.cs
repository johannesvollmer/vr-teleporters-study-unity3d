using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour {

	public Color baseColor;
	public Color grabColor;

	void Start(){
		Deactivate ();
	}

	public void Activate(){
		GetComponent<MeshRenderer> ().material.color = grabColor;
	}

	public void Deactivate(){
		GetComponent<MeshRenderer> ().material.color = baseColor;
	}
}
