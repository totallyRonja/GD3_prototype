using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Enemy : Hitable {
	[Header("Freeze behaviour")]
    public float frozenTime = 1;
    [Range(0, 1)] public float frozenSpeed = 0.5f; //relative speed when frozen; 1 is no effect 0 is stopped
    public Material defaultMaterial;
    public Material frozenMaterial;

    protected bool frozen = false;
    protected Renderer render;
    Color baseColor;

    public void Freeze(){
        StopCoroutine("UnFreeze");
        render.material = frozenMaterial;
        StartCoroutine(UnFreeze(frozenTime));
        frozen = true;
    }

	public IEnumerator UnFreeze(float delay){
        yield return new WaitForSeconds(delay);
        render.material = defaultMaterial;
        frozen = false;
    }

	protected void FrozenStart(){
        render = GetComponentInChildren<Renderer>();
    }

    public bool inRange(Vector3 location, float distance, bool inside = true, bool heightCheck = true)
    {
        if (heightCheck && Mathf.Abs(location.y - transform.position.y) > 0.25f)
            return !inside;
        bool isInside = Vector3.Distance(transform.position, location) < distance;
        return inside ? isInside : !isInside;
    }
}

[Serializable]
public enum EnemyState
{
    Idling, //not doing anything
    Attacking, //shooting
    Following, //getting in shooting range
    Fleeing, //running away
    Shooting, //currently shooting
    Dying //you know what's up
}