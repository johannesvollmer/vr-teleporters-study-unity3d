using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Utility;
using HTC.UnityPlugin.Vive;
using HTC.UnityPlugin.PoseTracker;

public class DepthTeleporter : MonoBehaviour {

	public Transform cameraRigTansform;
	public Transform groundDisplay;

	public ControllerAxis activationAxis = ControllerAxis.Trigger;
	public ControllerButton activationTrigger = ControllerButton.FullTrigger;

	public float sensitivity = 20f;
	public float maxDistance = 40f;
	public Color color = new Color(1,1,1);
	public AnimationCurve transparencyCurve = new AnimationCurve(new Keyframe[]{ new Keyframe(0, 0), new Keyframe(1, 1) });
	private float maxLightIntensity;

	void Start(){
		maxLightIntensity = GetComponent<Light> ().intensity;
	}

	void Update () {
		if (VivePose.IsValidEx (HandRole.LeftHand) && VivePose.IsValidEx(HandRole.RightHand)) {
			float leftActivation = ViveInput.GetAxis (HandRole.LeftHand, activationAxis);
			float rightActivation = ViveInput.GetAxis (HandRole.RightHand, activationAxis);
			bool activated = leftActivation > 0.05 ||  rightActivation > 0.05;

			var line = gameObject.GetComponent<LineRenderer> ();
			var mesh = gameObject.GetComponent<MeshRenderer> ();
			var groundMesh = groundDisplay.GetComponent<MeshRenderer> ();
			var light = gameObject.GetComponent<Light> ();

			mesh.enabled = activated;
			line.enabled = activated;
			groundMesh.enabled = activated;
			light.enabled = activated;

			if (activated) {
				var activation = Mathf.Max (leftActivation, rightActivation);
				var primaryHand = rightActivation > leftActivation ? HandRole.RightHand : HandRole.LeftHand;
				var secondaryHand = rightActivation > leftActivation ? HandRole.LeftHand : HandRole.RightHand;

				float displayIntensity = transparencyCurve.Evaluate (activation - 0.05f);
				color.a = displayIntensity;
				mesh.material.SetColor("_TintColor", color);
				line.material.SetColor("_TintColor", color);

				color.a *= 0.4f;
				groundMesh.material.SetColor("_TintColor", color);
				light.intensity = maxLightIntensity * displayIntensity;


				var localPrimary = VivePose.GetPoseEx (primaryHand).pos;
				var primary = cameraRigTansform.TransformPoint (localPrimary);
				var secondary = cameraRigTansform.TransformPoint (VivePose.GetPoseEx (secondaryHand).pos);

				var direction = primary - secondary;
				var distance = (Mathf.Pow (2, direction.magnitude * sensitivity) - 1);
				var target = primary + direction.normalized * Mathf.Min(distance, maxDistance);
				var groundTarget = target - localPrimary;

				gameObject.transform.position = target;
				groundDisplay.position = new Vector3(target.x, groundTarget.y, target.z);
				line.SetPositions (new Vector3[]{ secondary, target });

				if (ViveInput.GetPressDown (primaryHand, activationTrigger)) {
					cameraRigTansform.position = groundTarget;
				}
			}
			else throw new UnityException("Invalid Hand Roles");
		}
	}

}
