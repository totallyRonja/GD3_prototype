using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTrigger : MonoBehaviour {
    public string transitionToLevel;

	void OnTriggerEnter(Collider coll){
		if(coll.CompareTag("Player")){
            SceneManager.LoadScene(transitionToLevel);
        }
	}
}
