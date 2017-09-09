using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StackableToggle : MonoBehaviour {

	void Start()
    {
        Toggle checkbox = GetComponent<Toggle>();
        checkbox.onValueChanged.AddListener((bool value) => StaticInfo.stackableAbilities = value);
        checkbox.isOn = StaticInfo.stackableAbilities;
    }
}
