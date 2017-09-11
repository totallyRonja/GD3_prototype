using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldPlayer : MonoBehaviour
{

    public static OverworldPlayer current;
    public static WorldNode currentNode;

    public WorldNode startNode;

    bool isMoving = false;

    void Start()
    {
        current = this;
        if (currentNode == null)
        {
            currentNode = startNode;
        }
        transform.position = currentNode.transform.position;
    }

    void Update()
    {
        if (isMoving)
            return;

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (input.magnitude > .9f)
        {
            foreach (Connection con in currentNode.connections)
            {
                float scalar = Vector2.Dot(input.normalized, con.direction.normalized);
                if (scalar > 0.5f)
                {
                    StartCoroutine(Wander(con.node));
                    break;
                }
            }
        }

        if (!string.IsNullOrEmpty(currentNode.sceneName) &&
                ((Input.GetButtonDown("MouseAction") &&
                ((Input.mousePosition - ((RectTransform)transform).position) / Screen.height).magnitude < 0.2f)) || (Input.GetButtonDown("ControllerAction")))
        {
            SceneManager.LoadScene(currentNode.sceneName);
        }
    }

    public void GoTo(WorldNode toNode)
    {
        if (toNode != currentNode && !isMoving)
        {
            StartCoroutine(Wander(toNode));
            print("Go To " + toNode.name);
        }
    }

    public IEnumerator Wander(WorldNode toNode)
    {
        isMoving = true;
        float startTime = Time.unscaledTime;
        while (Time.unscaledTime < startTime + 1)
        {
            yield return null;
            transform.position = Vector2.Lerp(currentNode.transform.position, toNode.transform.position, Time.unscaledTime - startTime);
        }
        currentNode = toNode;
        isMoving = false;
    }
}
