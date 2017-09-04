using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour {

	public void SetBoomerang(bool boomerangState){
        Bullet.boomerang = boomerangState;
    }

    public void SetCharge(bool boomerangState)
    {
        Debug.LogWarning("charge not implemented");
    }

    public void SetUpgrade(bool boomerangState)
    {
        Debug.LogWarning("3rd Upgrade not implemented");
    }
}
