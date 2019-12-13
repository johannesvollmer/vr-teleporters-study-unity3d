using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Utility;
using HTC.UnityPlugin.Vive;
using HTC.UnityPlugin.PoseTracker;

public class Dragger : MonoBehaviour {

	public Transform cameraRigTansform;

	public ControllerAxis activationAxis = ControllerAxis.Trigger;
	public ControllerButton activationTrigger = ControllerButton.FullTrigger;

	public Color color = new Color(1,1,1);
	public AnimationCurve transparencyCurve = new AnimationCurve(new Keyframe[]{ new Keyframe(0, 0), new Keyframe(1, 1) });

	void FixedUpdate () {
		float leftActivation = ViveInput.GetAxis (HandRole.LeftHand, activationAxis);
		float rightActivation = ViveInput.GetAxis (HandRole.RightHand, activationAxis);
		bool activated = leftActivation > 0.05 ||  rightActivation > 0.05;

		var line = gameObject.GetComponent<LineRenderer> ();
		var light = gameObject.GetComponent<Light> ();

		line.enabled = activated;
		light.enabled = activated;

		if (activated) {
			var hand = rightActivation > leftActivation ? HandRole.RightHand : HandRole.LeftHand;

			if (VivePose.IsValidEx (hand)) {
				var activation = Mathf.Max (leftActivation, rightActivation);

				float displayIntensity = transparencyCurve.Evaluate (activation - 0.05f);
				color.a = displayIntensity;
				line.material.SetColor("_TintColor", color);

				var pose = VivePose.GetPoseEx (hand);
				var poseWorldPosition = cameraRigTansform.TransformPoint (pose.pos);

				gameObject.transform.position = poseWorldPosition;

				if (ViveInput.GetPressDown (hand, activationTrigger)) {
					cameraRigTansform.position += poseWorldPosition;
				}
			}
			else throw new UnityException("Invalid Hand Roles");
		}
	}
}
