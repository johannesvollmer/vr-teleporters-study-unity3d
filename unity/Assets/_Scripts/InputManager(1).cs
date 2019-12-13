using HTC.UnityPlugin.Utility;
using HTC.UnityPlugin.VRModuleManagement;
using System;
using UnityEngine;
using UnityEngine.Events;
using HTC.UnityPlugin.Vive;

public class InputManager: MonoBehaviour {

	[Header("Setup")]
	public bool isRighthanded = true;
	private HandRole PrimaryHand = HandRole.RightHand;
	private HandRole SecondaryHand = HandRole.LeftHand;

	//Inputs:
	//Menu Button (Press)
	[HideInInspector]
	public ControllerButton MenuButton = ControllerButton.Menu;
	//Grip Button (Touch[not useful], Press)
	[HideInInspector]
	public ControllerButton GripButton = ControllerButton.Grip;
	//Trigger (Axis[1D], Press)
	[HideInInspector]
	public ControllerButton TriggerButton = ControllerButton.FullTrigger;
	[HideInInspector]
	public ControllerAxis TriggerAxis = ControllerAxis.Trigger;
	//Touchpad (Axis[2D], Press)
	[HideInInspector]
	public ControllerButton TouchpadButton = ControllerButton.Pad;

	//Primary Hand:
	[Header("P Menu")]
	public bool P_Menu_PressDown;
	public bool P_Menu_Press;
	public bool P_Menu_PressUp;

	[Header("P Grip")]
	public bool P_Grip_PressDown;
	public bool P_Grip_Press;
	public bool P_Grip_PressUp;

	[Header("P Trigger")]
	[Range(0.0f,1.0f)]
	public float P_Trigger_Pos;
	public bool P_Trigger_PressDown;
	public bool P_Trigger_Press;
	public bool P_Trigger_PressUp;

	[Header("P Touchpad")]
	public Vector2 P_Pad_Pos;
	public Vector2 P_Pad_Pos_Last;
	public bool P_Pad_PressDown;
	public bool P_Pad_Press;
	public bool P_Pad_PressUp;

	[Header("P Touchpad Regions")]
	public float P_Deadzone = 0.2f;
	public bool P_Pad_Top;
	public bool P_Pad_Right;
	public bool P_Pad_Bottom;
	public bool P_Pad_Left;

	//Secondary Hand:
	[Header("S Menu")]
	public bool S_Menu_PressDown;
	public bool S_Menu_Press;
	public bool S_Menu_PressUp;

	[Header("S Grip")]
	public bool S_Grip_PressDown;
	public bool S_Grip_Press;
	public bool S_Grip_PressUp;

	[Header("S Trigger")]
	[Range(0.0f,1.0f)]
	public float S_Trigger_Pos;
	public bool S_Trigger_PressDown;
	public bool S_Trigger_Press;
	public bool S_Trigger_PressUp;

	[Header("S Touchpad")]
	public Vector2 S_Pad_Pos;
	public Vector2 S_Pad_Pos_Last;
	public bool S_Pad_PressDown;
	public bool S_Pad_Press;
	public bool S_Pad_PressUp;

	[Header("S Touchpad Regions")]
	public float S_Deadzone = 0.2f;
	public bool S_Pad_Top;
	public bool S_Pad_Right;
	public bool S_Pad_Bottom;
	public bool S_Pad_Left;

	//Functions:
	void start (){
		SetHandRole ();
	}

	void Update () {

		SetHandRole ();

		//Primary Hand
		P_Menu_PressDown = PressDown (PrimaryHand, MenuButton);
		P_Menu_Press = Press (PrimaryHand, MenuButton);
		P_Menu_PressUp = PressUp (PrimaryHand, MenuButton);

		P_Grip_PressDown = PressDown (PrimaryHand, GripButton);
		P_Grip_Press = Press (PrimaryHand, GripButton);
		P_Grip_PressUp = PressUp (PrimaryHand, GripButton);

		P_Trigger_Pos = Axis1D (PrimaryHand, TriggerAxis, false);
		P_Trigger_PressDown = PressDown (PrimaryHand, TriggerButton);
		P_Trigger_Press = Press (PrimaryHand, TriggerButton);
		P_Trigger_PressUp = PressUp (PrimaryHand, TriggerButton);

		P_Pad_Pos = Axis2D (PrimaryHand, false);
		P_Pad_Pos_Last = Axis2D (PrimaryHand, true);
		P_Pad_PressDown = PressDown (PrimaryHand, TouchpadButton);
		P_Pad_Press = Press (PrimaryHand, TouchpadButton);
		P_Pad_PressUp = PressUp (PrimaryHand, TouchpadButton);

		//Secondary Hand
		S_Menu_PressDown = PressDown (SecondaryHand, MenuButton);
		S_Menu_Press = Press (SecondaryHand, MenuButton);
		S_Menu_PressUp = PressUp (SecondaryHand, MenuButton);

		S_Grip_PressDown = PressDown (SecondaryHand, GripButton);
		S_Grip_Press = Press (SecondaryHand, GripButton);
		S_Grip_PressUp = PressUp (SecondaryHand, GripButton);

		S_Trigger_Pos = Axis1D (SecondaryHand, TriggerAxis, false);
		S_Trigger_PressDown = PressDown (SecondaryHand, TriggerButton);
		S_Trigger_Press = Press (SecondaryHand, TriggerButton);
		S_Trigger_PressUp = PressUp (SecondaryHand, TriggerButton);

		S_Pad_Pos = Axis2D (SecondaryHand, false);
		S_Pad_Pos_Last = Axis2D (SecondaryHand, true);
		S_Pad_PressDown = PressDown (SecondaryHand, TouchpadButton);
		S_Pad_Press = Press (SecondaryHand, TouchpadButton);
		S_Pad_PressUp = PressUp (SecondaryHand, TouchpadButton);

		SetPadRegions (P_Pad_Pos, P_Pad_Pos_Last, P_Deadzone, false);
		SetPadRegions (S_Pad_Pos, S_Pad_Pos_Last, S_Deadzone, false);
	}

	private bool PressDown (HandRole hand, ControllerButton button){
		return ViveInput.GetPressDownEx (hand, button);
	}
	private bool Press (HandRole hand, ControllerButton button){
		return ViveInput.GetPressEx (hand, button);
	}
	private bool PressUp (HandRole hand, ControllerButton button){
		return ViveInput.GetPressUpEx (hand, button);
	}
	private float Axis1D (HandRole hand, ControllerAxis axis, bool getLastPos){
		return ViveInput.GetAxisEx (hand, axis, getLastPos);
	}
	private Vector2 Axis2D (HandRole hand, bool getLastPos){
		return ViveInput.GetPadAxisEx (hand, getLastPos);
	}
	private void SetHandRole(){
		if (isRighthanded) {
			PrimaryHand = HandRole.RightHand;
			SecondaryHand = HandRole.LeftHand;
		} else {
			PrimaryHand = HandRole.LeftHand;
			SecondaryHand = HandRole.RightHand;
		}
	}
	private void SetPadRegions(Vector2 Pad_Pos, Vector2 Pad_Pos_Last,float Deadzone, bool useLast){
		Vector2 Pos;
		if (useLast) { Pos = Pad_Pos_Last; }
		else { Pos = Pad_Pos; }

		P_Pad_Top = false;
		P_Pad_Right = false;
		P_Pad_Bottom = false;
		P_Pad_Left = false;

		if (Pos.y > Deadzone || -Pos.y > Deadzone || Pos.x > Deadzone || -Pos.x > Deadzone) {
			if ((Pos.x > Pos.y && Pos.y >= 0) || (Pos.x > -Pos.y && Pos.y < 0))
				P_Pad_Right = true;
			if ((-Pos.x > Pos.y && Pos.y >= 0) || (-Pos.x > -Pos.y && Pos.y < 0))
				P_Pad_Left = true;
			if ((Pos.y > Pos.x && Pos.x >= 0) || (Pos.y > -Pos.x && Pos.x < 0))
				P_Pad_Top = true;
			if ((-Pos.y > Pos.x && Pos.x >= 0) || (-Pos.y > -Pos.x && Pos.x < 0))
				P_Pad_Bottom = true;
		}
	}
}