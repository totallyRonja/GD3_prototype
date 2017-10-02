using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseDamp : MonoBehaviour {

	Image image;

	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        image.color = new Color(1, 1, 1, 1 - Time.timeScale);
    }
}
