using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour {

	public void SetReturning(bool changeTo){
        Bullet.returning = changeTo;
        //print("returning now "+changeTo);
    }

    public void SetCharge(bool changeTo)
    {
        Bullet.chargeable = changeTo;
    }

    public void SetFreezing(bool changeTo)
    {
        Bullet.freezing = changeTo;
    }
}
