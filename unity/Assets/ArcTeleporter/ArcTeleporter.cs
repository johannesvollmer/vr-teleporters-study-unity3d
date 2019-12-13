using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Utility;
using HTC.UnityPlugin.Vive;
using HTC.UnityPlugin.PoseTracker;

public class ArcTeleporter : MonoBehaviour {

	public Transform cameraRigTansform;
	public Transform groundDisplay;

	public ControllerAxis activationAxis = ControllerAxis.Trigger;
	public ControllerButton activationTrigger = ControllerButton.FullTrigger;

	public float power = 1;
	public float gravity = 0.01f;

	public float maxArcLength = 20;
	public int maxSteps = 120;

	public float wallLightDistance = 0.2f;
	public float wallGroundDisplayDistance = 0.02f;

	public Color color = new Color(1,1,1);
	public AnimationCurve transparencyCurve = new AnimationCurve(new Keyframe[]{ new Keyframe(0, 0), new Keyframe(1, 1) });
	private float maxLightIntensity;

	void Start(){
		maxLightIntensity = GetComponent<Light> ().intensity;
	}

	void FixedUpdate () {
		float leftActivation = ViveInput.GetAxis (HandRole.LeftHand, activationAxis);
		float rightActivation = ViveInput.GetAxis (HandRole.RightHand, activationAxis);
		bool activated = leftActivation > 0.05 ||  rightActivation > 0.05;

		var line = gameObject.GetComponent<LineRenderer> ();
		//var mesh = gameObject.GetComponent<MeshRenderer> ();
		var groundMesh = groundDisplay.GetComponent<MeshRenderer> ();
		var light = gameObject.GetComponent<Light> ();

		//mesh.enabled = activated;
		line.enabled = activated;
		groundMesh.enabled = activated;
		light.enabled = activated;

		if (activated) {
			var hand = rightActivation > leftActivation ? HandRole.RightHand : HandRole.LeftHand;

			if (VivePose.IsValidEx (hand)) {
				var activation = Mathf.Max (leftActivation, rightActivation);

				float displayIntensity = transparencyCurve.Evaluate (activation - 0.05f);
				color.a = displayIntensity;
				//mesh.material.SetColor("_TintColor", color);
				line.material.SetColor("_TintColor", color);

				color.a *= 0.4f;
				groundMesh.material.SetColor("_TintColor", color);
				light.intensity = maxLightIntensity * displayIntensity;

				var pose = VivePose.GetPoseEx (hand);
				var poseWorldPosition = cameraRigTansform.TransformPoint (pose.pos);


				var worldPosition = poseWorldPosition;
				var worldVelocity = cameraRigTansform.TransformDirection (pose.rot * (Vector3.forward * power));
				var distance = 0f;

				var positions = new List<Vector3>();
				Vector3? normal = null;

				for (int i = 0; i < maxSteps; i++) {
					positions.Add(worldPosition);

					var stepSize = (i + 1) * 0.05f; // make larger steps the further we already are
					var step = worldVelocity * stepSize;
					var stepLength = step.magnitude;


					RaycastHit hit;
					// raycast that discrete part of the arc, ignoring triggers
					if (Physics.Raycast (worldPosition, worldVelocity.normalized, out hit, stepLength, Physics.DefaultRaycastLayers,  QueryTriggerInteraction.Ignore)) {
						normal = hit.normal;
						positions.Add (hit.point);
						break;
					} 
					else if (distance + stepLength > maxArcLength) {
						positions.Add (worldPosition + worldVelocity.normalized * (maxArcLength - distance));
						break;
					}

					distance += stepLength;
					worldPosition += step;
					worldVelocity += Physics.gravity * (gravity * stepSize);
				}

				var target = positions[positions.Count - 1];

				var lightDisplayTarget = target;
				var groundDisplayTarget = target;
				if (normal.HasValue) {
					lightDisplayTarget += normal.Value.normalized * wallLightDistance;
					groundDisplayTarget += normal.Value.normalized * wallGroundDisplayDistance;
				}
				
				gameObject.transform.position = lightDisplayTarget;
				groundDisplay.position = groundDisplayTarget;

				line.positionCount = positions.Count;
				line.SetPositions (positions.ToArray());

				if (ViveInput.GetPressDown (hand, activationTrigger)) {
					// for perfectly vertical pointing, do not translate (by relativizing to controller only on horizontal axes)
					var relativeTranslation = target - new Vector3(poseWorldPosition.x, cameraRigTansform.position.y, poseWorldPosition.z);
					cameraRigTansform.position += relativeTranslation;
				}
			}
			else throw new UnityException("Invalid Hand Roles");
		}
	}
}
