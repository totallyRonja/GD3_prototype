using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField] float speed;
    Rigidbody rigid;

    // Use this for initialization
    void Start () {
        rigid = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        rigid.MovePosition(rigid.position + transform.forward * Time.deltaTime * speed);
    }
}
