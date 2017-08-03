using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] bool destroyOnHit;
    [SerializeField] float lifetime;
    Rigidbody rigid;
    float startTime;
    Vector3 pos;

    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        rigid.velocity = transform.forward * speed;
        if (Time.time - startTime > lifetime)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            //print(gameObject.name + " met a wall");
            if (destroyOnHit)
                Destroy(gameObject);
        } else {
            Hitable hpComponent = coll.gameObject.GetComponent<Hitable>();
            if(hpComponent){
                hpComponent.Hit(1);
                if(destroyOnHit)
                    Destroy(gameObject);
            }
        }
    }
}
