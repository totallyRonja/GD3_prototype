using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChargerEnemy : Enemy
{

    [Header("General stuff")]
    public EnemyState state = EnemyState.Idling;
    public int maxHp = 3;
    public Animator anim;

    [Header("Idle")]
    public float walkRadius;
    public float newPointDelay;
    public float aggroRadius;

    [Header("Movement")]
    public float walkSpeed;

    [Header("Attack")]
    public float chargeSpeed;
    public int damage;
    public float waitTime;

    NavMeshAgent agent;
    CharacterController controller;

    Vector3 startPoint;
    bool armored = false;
    float startCharging;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();

        agent.speed = walkSpeed;
        startPoint = transform.position;

        health = maxHp;

        TransitionTo(state);

        FrozenStart();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case EnemyState.Idling:
                Idle();
                break;
            case EnemyState.Following:
                Follow();
                break;
            case EnemyState.Attacking:
                Attack();
                break;
            case EnemyState.Dying:
                Die();
                break;
        }
    }

    void Idle()
    {
        agent.speed = walkSpeed * (frozen ? frozenSpeed : 1);

        if (inRange(Player.current.transform.position, aggroRadius))
        {
            TransitionTo(EnemyState.Following);
        }
    }

    void Follow()
    {
        agent.speed = walkSpeed * (frozen ? frozenSpeed : 1);
        agent.SetDestination(Player.current.transform.position);
        float facing = Vector3.Dot(transform.forward, Vector3.Normalize(Player.current.transform.position - transform.position));
        if (facing > 0.99f)
        {
            TransitionTo(EnemyState.Attacking);
        }
        else if (inRange(Player.current.transform.position, aggroRadius * 1.5f, false, false))
        {
            TransitionTo(EnemyState.Idling);
        }

    }

    void Attack()
    {
        Vector3 direction = Player.current.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, angle, Time.deltaTime * 30);
        if (Time.time >= startCharging + waitTime)
        {
            Vector3 velocity = transform.forward * chargeSpeed * (frozen ? frozenSpeed : 1);
            velocity.y = -1;
            controller.Move(velocity * Time.deltaTime);
        }
    }

    void Die()
    {
        transform.Translate(0, -Time.deltaTime, 0);
        if (Time.time > startCharging + 3)
        {
            Destroy(gameObject);
        }
    }

    void TransitionTo(EnemyState newState)
    {
        //leave old state
        switch (state)
        {
            case EnemyState.Idling:
                CancelInvoke("GoToRandom");
                break;
            case EnemyState.Following:
                break;
            case EnemyState.Attacking:
                break;
        }
        state = newState;
        //enter new state
        switch (state)
        {
            case EnemyState.Idling:
                agent.enabled = true;
                controller.enabled = false;
                InvokeRepeating("GoToRandom", 0, newPointDelay);
                SetArmor(false);
                break;
            case EnemyState.Following:
                agent.enabled = true;
                controller.enabled = false;
                SetArmor(true);
                break;
            case EnemyState.Attacking:
                agent.enabled = false;
                controller.enabled = true;
                startCharging = Time.time;
                break;
            case EnemyState.Dying:
                agent.enabled = false;
                controller.enabled = false;
                startCharging = Time.time;
                break;
        }
    }

    void GoToRandom()
    {
        Vector2 flatpoint = Random.insideUnitCircle * walkRadius;
        Vector3 offset = new Vector3(flatpoint.x, 0, flatpoint.y);
        agent.SetDestination(startPoint + offset);
    }

    void SetArmor(bool armorState)
    {
        armored = armorState;
        anim.SetBool("Armored", armorState);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.normal.y < 0.1f)
        {
            Hitable targetHitable = hit.gameObject.GetComponent<Hitable>();
            if (targetHitable)
            {
                targetHitable.Hit(damage);
            }
            TransitionTo(EnemyState.Idling);

        }
    }

    void OnDrawGizmosSelected()
    {
        //idle walk area
        Gizmos.color = Color.blue;
        Gizmos.matrix = Matrix4x4.Translate(startPoint != Vector3.zero ? startPoint : transform.position) *
                Matrix4x4.Scale(new Vector3(1, 0.1f / walkRadius, 1));
        Gizmos.DrawWireSphere(Vector3.zero, walkRadius);

        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.Translate(transform.position) *
                Matrix4x4.Scale(new Vector3(1, 0.1f / aggroRadius, 1));
        Gizmos.DrawWireSphere(Vector3.zero, aggroRadius);
    }

    public override bool Hit(int damage, Vector3 damagePoint = new Vector3())
    {
        if (!armored || Vector3.Dot((transform.position - damagePoint).normalized, transform.forward) > 0)
        {
            health -= damage;
            TransitionTo(EnemyState.Attacking);
            startCharging = Time.time - waitTime;

        }

        if (health <= 0)
        {
            TransitionTo(EnemyState.Dying);
        }

        return true;
    }
}
