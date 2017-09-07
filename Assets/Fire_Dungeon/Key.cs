using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

	void OnTriggerEnter(Collider coll){
		if(coll.CompareTag("Player")){
            KeyManager.current.collectKey();
            gameObject.SetActive(false);
        }
	}
}
