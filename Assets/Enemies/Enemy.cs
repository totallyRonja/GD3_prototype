using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Hitable {
	[Header("Freeze behaviour")]
    public float frozenTime = 1;
    [Range(0, 1)] public float frozenSpeed = 0.5f; //relative speed when frozen; 1 is no effect 0 is stopped
    public Material defaultMaterial;
    public Material frozenMaterial;

    protected float frozen = 0; //frozen when > 0; count down 1 per second
    protected Renderer render;
    Color baseColor;

    public void Freeze(){
        frozen = frozenTime;
        render.material = frozenMaterial;
    }

	public void UnFreeze(){
		render.material = defaultMaterial;
	}

	protected void FrozenStart(){
        render = GetComponentInChildren<Renderer>();
    }

	protected void FrozenUpdate(){
        if (frozen > 0)
        {
            frozen -= Time.deltaTime;
            if(frozen <= 0){
                UnFreeze();
            }
        }
    }
}
