using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class MethodManager : MonoBehaviour {

	public InputManager Input;

	public GameObject[] Method;

	public Image[] Icon;
	public Color activeColorIcon;
	public Color inactiveColorIcon;

	public Image[] BG;
	public Color activeColorBG;
	public Color inactiveColorBG;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		int i = GetRLTB (Input.S_Pad_Right, Input.S_Pad_Left, Input.S_Pad_Top, Input.S_Pad_Bottom);
		if (i == -1) {
			Disable_All_Methods ();
			if (Input.S_Pad_Pos != new Vector2 (0f, 0f)) {
				Set_One_Active_Color (Icon, 4, activeColorIcon, inactiveColorIcon);
			} 
			else {
				Set_All_Inactive_Color (Icon, inactiveColorIcon);
			}
			Set_All_Inactive_Color (BG, inactiveColorBG);
		}
		else {
			Enable_One_Method (i);
			Set_One_Active_Color (Icon, i, activeColorIcon, inactiveColorIcon);
			Set_One_Active_Color (BG, i, activeColorBG, inactiveColorBG);
		}
	}

	private void Enable_One_Method (int z){
		Disable_All_Methods ();
		Method [z].SetActive (true);
	}
	private void Disable_All_Methods(){
		int a = Method.Length;
		for(int i = 0; i < a; i++){
			Method [i].SetActive (false);
		}
	}
	private void Set_One_Active_Color (Image[] image, int z, Color A, Color IA){
		Set_All_Inactive_Color(image, IA);
		image [z].color = A;
	}
	private void Set_All_Inactive_Color(Image[] image, Color IA){
		int a = image.Length;
		for(int i = 0; i < a; i++){
			image [i].color = IA;
		}
	}
	private int GetRLTB(bool right, bool left, bool top, bool bottom){
		if (right)
			return 0;
		if (left)
			return 1;
		if (top)
			return 2;
		if (bottom)
			return 3;
		else {
			return -1;
		}
	}
}
