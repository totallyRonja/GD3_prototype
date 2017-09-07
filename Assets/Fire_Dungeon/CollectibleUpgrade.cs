using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleUpgrade : MonoBehaviour {

    public UpgradeType upgrade;

	void Update(){
        transform.Rotate(0, Time.deltaTime * 360, 0);
    }

    void OnTriggerEnter(Collider coll){
        gameObject.SetActive(false);
        switch(upgrade){
			case UpgradeType.RETURN:
                Bullet.returning = true;
                break;
			case UpgradeType.CHARGE:
				
                break;
			case UpgradeType.TODO:

                break;
			default:
                Debug.LogWarning("Invalid collectible type");
                break;
        }
	}
}

public enum UpgradeType{
	RETURN,
	CHARGE,
	TODO
}
