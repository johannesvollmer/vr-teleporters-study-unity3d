using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	private Color originalColor;
	public Color activeColor = new Color(1,1,1);

	void Start () {
		originalColor = gameObject.GetComponent<MeshRenderer> ().material.color;
	}


	public void activate(){
		gameObject.GetComponent<MeshRenderer> ().material.color = activeColor;
	}

	public void deactivate(){
		gameObject.GetComponent<MeshRenderer> ().material.color = originalColor;
	}
}
