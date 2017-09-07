﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static bool returning = false;
    public static bool chargeable = false;
    public static bool freezing = false;

    public float speed;
    public float lifetime;
    public float damage = 1;
    public Material frozenMaterial;
    public Mesh returningMesh;

    CharacterController controller;
    float startTime;
    Vector3 direction;
    Transform player;
    Transform mesh;

    // Use this for initialization
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        startTime = Time.time;
        player = Player.current.transform;
        direction = transform.forward;
        mesh = GetComponentInChildren<Renderer>().transform;

        if(freezing){
            mesh.GetComponent<Renderer>().material = frozenMaterial;
        }
        if(returning){
            MeshFilter mf = mesh.GetComponent<MeshFilter>();
            mf.mesh = returningMesh;
            mf.transform.localScale = Vector3.one * 2.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(returning){
            Vector3 flyForward = direction * speed * Time.deltaTime * 1.1f;
            Vector3 flyToPlayer = ((player.position+Vector3.up * 1.5f) - transform.position).normalized * speed * Time.deltaTime * (Time.time - startTime);
            Vector3 flyDirection = flyForward + flyToPlayer;
            controller.Move(flyDirection);
            //transform.localEulerAngles = new Vector3(0, Mathf.Atan2(flyDirection.x, flyDirection.z) * Mathf.Rad2Deg, 0);

            mesh.localEulerAngles = new Vector3(0, Time.time * 720, 0);
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
            hpComponent.Hit(Mathf.FloorToInt(damage));
            if(freezing){
                Enemy enemy = coll.gameObject.GetComponent<Enemy>();
                if(enemy){
                    enemy.Freeze();
                }
            }
        }
        Destroy(gameObject);
    }
}
