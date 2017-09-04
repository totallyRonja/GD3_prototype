using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoor : Hitable {

	public override bool Hit(int damage){
        transform.parent.gameObject.SetActive(false);
        return true;
    }
}
