using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SniperEnemy : Enemy
{
    [Header("Current state")]
	public EnemyState state = EnemyState.Sleeping;

    [Header("Sleeping")]
    public float wakeUpDistance;

    [Header("Attacking")]
	public float minAttackDistance;
    public float maxAttackDistance;
    public LineRenderer line;
    public float shotDelay;

    [Header("Moving")]
	public float speed;

    [Header("Health")]
    public int maxHP;

    NavMeshAgent agent;
    float timeOfDeath = -1; //time of death
    //float lastShotTime = -1; //time of the last shot //replaced by IEnumerator
    //float yVelocity = 0;
    //float gravity = -20;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = maxHP;
        FrozenStart();
    }

    void Update()
    {
        FrozenUpdate();
        switch (state)
        {
            case EnemyState.Sleeping:
                Sleep();
                break;
            case EnemyState.Attacking:
                Attack();
                break;
            case EnemyState.Following:
                Follow();
                break;
            case EnemyState.Fleeing:
                Flee();
                break;
			case EnemyState.Dying:
                Die();
                break;
        }
    }

	void Sleep()
	{
		/*yVelocity += gravity * Time.deltaTime;
        agent.Move(new Vector3(0, yVelocity, 0));
        if (agent.isGrounded)
            yVelocity = 0;*/

        if (inRange(Player.current.transform.position, wakeUpDistance))
        {
            state = EnemyState.Following;
        }
	}

    void Follow()
    {
        agent.isStopped = false;
        /*Vector3 velocity = Player.current.transform.position - transform.position;
        velocity = velocity.normalized * speed;*/
        agent.speed = speed * (frozen>0?frozenSpeed:1);
        agent.destination = Player.current.transform.position;
        //transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg, Vector3.up);
        if (inRange(Player.current.transform.position, maxAttackDistance))
        {
            state = EnemyState.Attacking;
        }
    }

    void Flee()
    {
        agent.isStopped = true;
        Vector3 velocity = Player.current.transform.position - transform.position;
        velocity = velocity.normalized * speed * (frozen > 0 ? frozenSpeed : 1);
        agent.Move(-velocity * Time.deltaTime); //do the opposite of expected to run away
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg, Vector3.up);
        if ((transform.position - Player.current.transform.position).magnitude > minAttackDistance)
        {
            state = EnemyState.Attacking;
        }
    }

    void Attack()
    {
        //transition magic
        if ((transform.position - Player.current.transform.position).magnitude < minAttackDistance)
        {
            state = EnemyState.Fleeing;
        }
        else if ((transform.position - Player.current.transform.position).magnitude > maxAttackDistance)
        {
            state = EnemyState.Following;
        } else {
            state = EnemyState.Shooting;
            StartCoroutine(Shoot());
        }
    }

    void Die()
	{
        line.enabled = false;
        StopAllCoroutines();
        if(timeOfDeath < 0)
            timeOfDeath = Time.time;
        agent.enabled = false;
        transform.Translate(Vector3.down * Time.deltaTime);
		if(Time.time - timeOfDeath > 5)
            Destroy(gameObject);
    }

    IEnumerator Shoot(){
        //direction magic
        Vector3 direction = (Player.current.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg, Vector3.up);

        float distance = 100;
        line.enabled = true;
        line.widthMultiplier = 0.025f;
        line.SetPosition(1, Vector3.forward * distance);

        float startTime = Time.time;
        while(Time.time < startTime + shotDelay - 0.5f){
            yield return null;
            //stop agent if it's not falling
            if(!agent.isOnOffMeshLink)
                agent.isStopped = true;
        }
        //yield return new WaitForSeconds(1.5f);
        line.widthMultiplier = 0.25f;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, distance)){
            line.SetPosition(1, Vector3.forward * hit.distance);
            Hitable health = hit.collider.GetComponent<Hitable>();
            if(health){
                health.Hit(3);
            }
        }
        yield return new WaitForSeconds(0.25f);
        line.enabled = false;
        yield return new WaitForSeconds(0.25f);
        state = EnemyState.Attacking;
    }

    public override bool Hit(int damage)
    {
        if (state != EnemyState.Dying)
        {
            health -= damage;
            if (health <= 0)
            {
                state = EnemyState.Dying;
            } else {
				if(state == EnemyState.Sleeping)
                    state = EnemyState.Following;
            }
        }
        return true;
    }

    public bool inRange(Vector3 location, float distance, bool inside = true){
        if(Mathf.Abs(location.y - transform.position.y) > 2)
            return false;
        bool isInside = Vector3.Distance(transform.position, location) < distance;
        return inside ? isInside : !isInside;
    }
}

[Serializable]
public enum EnemyState
{
    Sleeping, //not doing anything
    Attacking, //shooting
    Following, //getting in shooting range
    Fleeing, //running away
    Shooting, //currently shooting
	Dying //you know what's up
}