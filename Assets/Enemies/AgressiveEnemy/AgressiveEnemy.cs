using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgressiveEnemy : Enemy
{
    [SerializeField]
    EnemyState state = EnemyState.Idling;

    [Header("Sleeping")]
    [SerializeField]
    float wakeUpDistance;
    [SerializeField]
    float sleepDistance;

    [Header("Attacking")]
    [SerializeField]
    float speed;
    [SerializeField]
    int damage;

    [Header("Health")]
    [SerializeField]
    int maxHP;

    float timeOfDeath = Mathf.NegativeInfinity;
    CharacterController controller;
    Vector3 velocity;

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();

        health = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case EnemyState.Idling:
				velocity.x = 0;
				velocity.y += -20 * Time.deltaTime;
                velocity.z = 0;
                controller.Move(velocity * Time.deltaTime);
                if (controller.isGrounded)
                    velocity.y = 0;

                Vector3 diff = Player.current.transform.position - transform.position;
                if (diff.magnitude < wakeUpDistance)
                    state = EnemyState.Attacking;
                break;

            case EnemyState.Attacking:
                diff = Player.current.transform.position - transform.position;
                transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg, Vector3.up);

                velocity.x = (transform.forward * speed).x;
				velocity.y += -20 * Time.deltaTime;
                velocity.z = (transform.forward * speed).z;
                controller.Move(velocity * Time.deltaTime);

                if (diff.magnitude > sleepDistance)
                    state = EnemyState.Idling;
                break;

            case EnemyState.Dying:
                if (timeOfDeath < 0)
                    timeOfDeath = Time.time;
                
                controller.enabled = false;
                transform.localScale = transform.localScale * (1 - Time.deltaTime);
                transform.Translate(Vector3.down * Time.deltaTime);
                if (Time.time - timeOfDeath > 4)
                    Destroy(gameObject);
                break;
            default:
                Debug.Log("Invalid state");
                state = EnemyState.Idling;
                break;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit coll)
    {
        Hitable hit = coll.gameObject.GetComponent<Hitable>();
        if (hit && state != EnemyState.Dying && coll.gameObject.CompareTag("Player"))
        {
            hit.Hit(damage);
            state = EnemyState.Dying;
        }
    }

    public override bool Hit(int damage, Vector3 point)
    {
        health -= damage;
        if (health <= 0)
        {
            state = EnemyState.Dying;
        }
        return true;
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.Translate(transform.position) * Matrix4x4.Scale(new Vector3(1, 0.1f / wakeUpDistance, 1));
        Gizmos.DrawWireSphere(Vector3.zero, wakeUpDistance);

        Gizmos.color = Color.magenta;
        Gizmos.matrix = Matrix4x4.Translate(transform.position) * Matrix4x4.Scale(new Vector3(1, 0.1f / sleepDistance, 1));
        Gizmos.DrawWireSphere(Vector3.zero, sleepDistance);
    }
}
