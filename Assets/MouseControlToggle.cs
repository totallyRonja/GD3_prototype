using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseControlToggle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Toggle checkbox = GetComponent<Toggle>();
        checkbox.onValueChanged.AddListener((bool value) => StaticInfo.mouseControls = value);
        checkbox.isOn = StaticInfo.mouseControls;
    }
}
