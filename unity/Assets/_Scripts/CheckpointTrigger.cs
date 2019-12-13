using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour {

	public Checkpoint[] checkpoints;
	private int next;


	void Start () {
		// rigidbody is required for trigger detection
		if (!gameObject.GetComponent<Rigidbody> ()) {
			gameObject.AddComponent<Rigidbody> ();
			gameObject.GetComponent<Rigidbody> ().isKinematic = true;
		}

		Reset ();
	}

	void Reset(){
		next = 0;
	}

	void Enter(Checkpoint checkpoint){
		if (checkpoint && next < checkpoints.Length && checkpoints[next].gameObject == checkpoint.gameObject) {
			if (next != 0) 
				checkpoints [next - 1].Deactivate ();

			next++;

			if (next < checkpoints.Length) {
				checkpoint.Activate ();
				checkpoints[next].Hint ();
			} else {
				checkpoint.Finish ();
			}
		}
	}

	void Exit(Checkpoint checkpoint){
	}

	void OnTriggerEnter(Collider other){
		Enter (other.gameObject.GetComponent<Checkpoint> ());
	}

}
