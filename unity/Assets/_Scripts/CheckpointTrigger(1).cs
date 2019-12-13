using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour {


	void Start () {
		// rigidbody is required for trigger detection
		if (!gameObject.GetComponent<Rigidbody> ()) {
			gameObject.AddComponent<Rigidbody> ();
			gameObject.GetComponent<Rigidbody> ().isKinematic = true;
		}
	}

	void OnTriggerEnter(Collider other){
		var checkpoint = other.gameObject.GetComponent<Checkpoint> ();
		if (checkpoint) checkpoint.activate();
	}

	void OnTriggerExit(Collider other){
		var checkpoint = other.gameObject.GetComponent<Checkpoint> ();
		if (checkpoint) checkpoint.deactivate ();
	}
}
