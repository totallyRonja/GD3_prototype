using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetCameraPosition : MonoBehaviour {
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = player.position + offset;
    }
}
