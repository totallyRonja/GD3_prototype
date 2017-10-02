using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Companion : MonoBehaviour {

    public Player player;
    public float aheadDistance;
    public GameObject selector;
	public float selectorWidth = 1;
    public float commandDistance = 10;
    public float cursorSpeed;

    private Vector3 prevPlayerPos;
    private NavMeshAgent agent;

    private Vector3 difference;

    private Vector3? target = null;
    private NavMeshAgent cursor;
    private bool aiming;

    // Use this for initialization
    void Start () {
		if(!player)
            player = Player.current;

        agent = GetComponent<NavMeshAgent>();
        cursor = selector.GetComponent<NavMeshAgent>();

        selector.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        //TagObject();
        MoveCursor();
        Move();
        //SecondStick();
    }

	void SecondStick(){
        Vector3 motion = new Vector3(Input.GetAxis("Horizontal2"),0,Input.GetAxis("Vertical2")) * agent.speed;
        agent.Move(motion * Time.deltaTime);
		if(motion.magnitude > 0.1f)
        	transform.eulerAngles = Vector3.up * Mathf.Atan2(motion.x, motion.z) * Mathf.Rad2Deg;
    }

    void MoveCursor(){
        Vector3 input = new Vector3(Input.GetAxis("Horizontal2"), 0, Input.GetAxis("Vertical2"));
        if (input.magnitude > 0.9f)
        {
            if(!aiming){
                aiming = true;
                cursor.Warp(Player.current.transform.position);
                selector.SetActive(true);
            }

            Time.timeScale = 0.05f;

            cursor.enabled = true;
            cursor.Move(input * cursorSpeed * Time.unscaledDeltaTime);
            cursor.enabled = false;

            target = selector.transform.position;
            agent.SetDestination(target.Value);
        }
        else
        {
            aiming = false;
            Time.timeScale = 1f;
        }
    }

    void TagObject(){
		Vector3 input = new Vector3(Input.GetAxis("Horizontal2"), 0, Input.GetAxis("Vertical2"));
		if(input.magnitude > 0.2f){
            Time.timeScale = 0.1f;
            Player.current.arrow.gameObject.SetActive(true);
            Player.current.arrow.eulerAngles = Vector3.up * Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg;

            GameObject[] taggables = GameObject.FindGameObjectsWithTag("Taggable");
            GameObject closestTaggableInFocus = null;
            float closestDistance = 0;
            foreach(GameObject taggable in taggables){
				if(Vector3.Dot(taggable.transform.position - Player.current.transform.position, input) <= 0){
                    continue;
                }
                float distance = Vector3.Distance(taggable.transform.position, Player.current.transform.position);
                float width = Mathf.Sin(Vector3.Angle(taggable.transform.position - Player.current.transform.position, 
					input) * Mathf.Deg2Rad) * distance;
				if(width > selectorWidth || distance > commandDistance) 
					continue;
				if(closestTaggableInFocus == null){
                    closestTaggableInFocus = taggable;
                    closestDistance = distance;
                    continue;
                }
				if(distance < closestDistance){
					closestTaggableInFocus = taggable;
                    closestDistance = distance;
				}
            }
            if (closestTaggableInFocus != null)
            {
                selector.SetActive(true);
                selector.transform.position = closestTaggableInFocus.transform.position;
                target = closestTaggableInFocus.transform.position;
                agent.SetDestination(target.Value);
            } else {
				selector.SetActive(false);
                target = null;
            }
        } else {
            Time.timeScale = 1f;
			Player.current.arrow.gameObject.SetActive(false);
        }
    }

    void Move(){
        if (target.HasValue && Vector3.Distance(Player.current.transform.position, target.Value) > commandDistance)
        {
            target = null;
            selector.SetActive(false);
        }
        if(!target.HasValue)
        	agent.SetDestination(AheadPosition());
    }

	Vector3 AheadPosition(){
		Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        return player.transform.position + player.transform.forward * (aheadDistance * (input.magnitude > 0.1f ? 1f : 0.4f));
    }

	void OnDrawGizmos(){
		if(!agent) return;
        Gizmos.color = Color.red;
        //Gizmos.DrawSphere(agent.destination, .2f);
    }
}
