using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletTypeUi : MonoBehaviour {

	public UpgradeType type;
    Image display;

    // Use this for initialization
    void Start () {
        display = GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        Color displayColor = display.color;
        displayColor.a = BulletTypeManager.ActiveBulletState(type)?1f:0.2f;
        display.color = displayColor;
    }
}
