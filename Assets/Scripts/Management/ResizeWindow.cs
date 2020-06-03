using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeWindow : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        Screen.SetResolution(720, 720, false);
	}

}
