using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static bool boomerang = false;

    [SerializeField] float speed;
    [SerializeField] float lifetime;
    CharacterController controller;
    float startTime;
    Vector3 direction;
    Transform player;

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();
        startTime = Time.time;
        player = Player.current.transform;
        direction = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if(boomerang){
            Vector3 flyForward = direction * speed * Time.deltaTime * 1.1f;
            Vector3 flyToPlayer = ((player.position+Vector3.up * 1.5f) - transform.position).normalized * speed * Time.deltaTime * (Time.time - startTime);
            Vector3 flyDirection = flyForward + flyToPlayer;
            controller.Move(flyDirection);
            transform.localEulerAngles = new Vector3(0, Mathf.Atan2(flyDirection.x, flyDirection.z) * Mathf.Rad2Deg, 0);
            //Debug.DrawLine(transform.position, transform.position+flyDirection*10, Color.red, 0, false);
        }else
        {
            controller.Move(direction * speed * Time.deltaTime);
            if (Time.time - startTime > lifetime)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit coll)
    {
        Hitable hpComponent = coll.gameObject.GetComponent<Hitable>();
        if(hpComponent && (coll.transform != player)){
            hpComponent.Hit(1);
        }
        Destroy(gameObject);
    }
}
