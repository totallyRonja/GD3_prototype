using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[SerializeField] float speed;
	[SerializeField] Transform muzzle;
    [SerializeField] GameObject bullet;
    Rigidbody rigid;

    // Use this for initialization
    void Start () {
        rigid = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        float inputMag = input.magnitude;
        if (inputMag > 0.1f)
        {
            rigid.rotation = Quaternion.AngleAxis(Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg, Vector3.up);
            rigid.MovePosition(rigid.position + transform.forward * inputMag * speed * Time.deltaTime);
        }
		if(Input.GetButtonDown("Fire1"))
		{
            Instantiate(bullet, muzzle.position, muzzle.rotation);
        }
    }
}
