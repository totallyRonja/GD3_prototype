using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour {

	[SerializeField] Hitable player;

    Slider bar;

    // Use this for initialization
    void Start () {
        bar = GetComponent<Slider>();
    }
	
	// Update is called once per frame
	void Update () {
        bar.value = player.health;
    }
}
