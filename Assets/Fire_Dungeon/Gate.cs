using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : Hitable {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override bool Hit(int damage){
		if(KeyManager.current.useKey()){
            gameObject.SetActive(false);
        }
        return true;
    }
}
