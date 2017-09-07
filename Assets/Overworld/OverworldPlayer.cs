using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldPlayer : MonoBehaviour {

    public WorldNode currentNode;

    bool isMoving = false;

    // Update is called once per frame
    void Update () {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		if(input.magnitude > .9f && !isMoving){
			foreach(Connection con in currentNode.connections){
                float scalar = Vector2.Dot(input.normalized, con.direction.normalized);
                print(scalar);
                if(scalar > 0.5f){
                    StartCoroutine(Wander(con.node));
                    break;
                }
            }
		}
    }

	IEnumerator Wander(WorldNode toNode){
        isMoving = true;
        float startTime = Time.unscaledTime;
		while(Time.unscaledTime < startTime + 1){
			yield return null;
            transform.position = Vector2.Lerp(currentNode.transform.position, toNode.transform.position, Time.unscaledTime - startTime);
        }
        currentNode = toNode;
        isMoving = false;
    }
}
