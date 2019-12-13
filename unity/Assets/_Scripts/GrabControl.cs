using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabControl : MonoBehaviour {

	public InputManager input;

	public float breakForce = 200;
	public float breakTorque = 200;

	private GrabObject hinted;
	private FixedJoint fixation;


	void Update(){
		if (input.P_Trigger_PressDown)
			GrabHinted();

		if (input.P_Trigger_PressUp || !input.P_Trigger_Press) // Sometimes, using only P_Trigger_PressUp does not work
			ReleaseGrabbed ();
	}

	void OnTriggerEnter(Collider collider){
		Hint (collider.GetComponent<GrabObject> ());
	}

	void OnTriggerExit(Collider collider){
		Unhint (collider.GetComponent<GrabObject> ());
	}

	void OnTriggerStay(Collider collider){
		if (!fixation) 
			Hint (collider.GetComponent<GrabObject> ());
	}


	void Hint(GrabObject grab){
		if (grab) {
			// only change current object if not currently grabbing
			if (!fixation) {
				if (!hinted || Vector3.Distance (transform.position, grab.transform.position) < Vector3.Distance (transform.position, hinted.transform.position)) {
					Unhint (hinted);
					hinted = grab;
					grab.Activate (); 

					if(input.P_Trigger_Press)
						GrabHinted();
				}
			}
		}
	}

	void Unhint(GrabObject grab){
		if (grab) {
			if (grab == hinted) {
				ReleaseGrabbed ();
				hinted.Deactivate ();
				hinted = null;
			}
		}
	}

	void GrabHinted(){
		if (hinted && !fixation) { 
			gameObject.AddComponent<FixedJoint> ();
			fixation = GetComponent<FixedJoint> ();
			fixation.breakForce = breakForce;
			fixation.breakTorque = breakTorque;
			fixation.connectedBody = hinted.GetComponent<Rigidbody> ();
		}
	}

	void ReleaseGrabbed(){
		if (fixation) {
			Vector3 velocity = hinted.GetComponent<Rigidbody> ().velocity;
			Destroy (fixation);
			fixation = null;

			if (hinted) {
				hinted.GetComponent<Rigidbody> ().AddForce (velocity);
			}
		}
	}

}
