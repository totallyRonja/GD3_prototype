using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class randomRotation : MonoBehaviour {
	void Start () {
        transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
    }
}
