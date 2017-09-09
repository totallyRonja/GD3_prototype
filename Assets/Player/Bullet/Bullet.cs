using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float lifetime;
    public float damage = 1;
    public Material frozenMaterial;
    public Mesh returningMesh;

    [HideInInspector] public CharacterController controller;
    float startTime;
    Transform player;
    [HideInInspector] public Transform mesh;

    // Use this for initialization
    public void Awake()
    {
        controller = GetComponent<CharacterController>();
        startTime = Time.time;
        player = Player.current.transform;
        mesh = GetComponentInChildren<Renderer>().transform;

        if(BulletTypeManager.freezing){
            mesh.GetComponent<Renderer>().material = frozenMaterial;
        }
        if(BulletTypeManager.returning){
            MeshFilter mf = mesh.GetComponent<MeshFilter>();
            mf.mesh = returningMesh;
            mf.transform.localScale = Vector3.one * 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(BulletTypeManager.returning){
            Vector3 flyForward = transform.forward * speed * Time.deltaTime * 1.1f;
            Vector3 flyToPlayer = ((player.position+Vector3.up * 1.5f) - transform.position).normalized * speed * Time.deltaTime * (Time.time - startTime);
            Vector3 flyDirection = flyForward + flyToPlayer;
            controller.Move(flyDirection);
            //transform.localEulerAngles = new Vector3(0, Mathf.Atan2(flyDirection.x, flyDirection.z) * Mathf.Rad2Deg, 0);

            mesh.localEulerAngles = new Vector3(0, Time.time * 720, 0);
        }else
        {
            controller.Move(transform.forward * speed * Time.deltaTime);
            if (Time.time - startTime > lifetime)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetPower(float power){
        damage = power;
        transform.localScale = Vector3.one * power;
    }

    void OnControllerColliderHit(ControllerColliderHit coll)
    {
        Hitable hpComponent = coll.gameObject.GetComponent<Hitable>();
        if(hpComponent && (coll.transform != player)){
            hpComponent.Hit(Mathf.FloorToInt(damage));
            if(BulletTypeManager.freezing){
                Enemy enemy = coll.gameObject.GetComponent<Enemy>();
                if(enemy){
                    enemy.Freeze();
                }
            }
        }
        Destroy(gameObject);
    }
}
