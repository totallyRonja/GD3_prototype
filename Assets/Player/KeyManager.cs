using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager current;

    public GameObject keyMesh;
    public float rotationSpeed = 1;
    public float scale = 1;
    int keys = 0;

    void Awake()
    {
        current = this;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < keys; i++)
        {
            float angle = Time.time * rotationSpeed + (2 * Mathf.PI * i) / keys;
            transform.GetChild(i).localPosition = new Vector3(Mathf.Sin(angle), 2, Mathf.Cos(angle));
            transform.GetChild(i).localScale = Vector3.one * scale;
        }
    }

    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }

    public void collectKey()
    {
        Instantiate(keyMesh, transform, false);
        keys++;
    }

    public bool useKey()
    {
        if (keys <= 0)
            return false;
        keys--;
        Destroy(transform.GetChild(0).gameObject);
        return true;
    }
}