using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	private Color originalColor = Color.white;
	public Color hintColor = Color.cyan;
	public Color activeColor = Color.green;
	public Color finishColor = Color.red;

	void Start () {
		originalColor = gameObject.GetComponent<MeshRenderer> ().material.color;
	}


	public void Activate(){
		gameObject.GetComponent<MeshRenderer> ().material.color = activeColor;
	}

	public void Deactivate(){
		gameObject.GetComponent<MeshRenderer> ().material.color = originalColor;
	}

	public void Finish(){
		gameObject.GetComponent<MeshRenderer> ().material.color = finishColor;
	}

	public void Hint(){
		gameObject.GetComponent<MeshRenderer> ().material.color = hintColor;
	}
}
