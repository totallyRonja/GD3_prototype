using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hitable : MonoBehaviour {
    [HideInInspector] public int health;
    public abstract bool Hit(int damage);
}
