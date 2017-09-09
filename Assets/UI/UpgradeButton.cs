using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour {
    public UpgradeType type;
    Toggle checkbox;

    void Awake(){
        checkbox = GetComponent<Toggle>();
        checkbox.onValueChanged.AddListener(SetUpgrade);

        switch (type)
        {
            case UpgradeType.RETURN:
                checkbox.isOn = StaticInfo.returning;
                break;
            case UpgradeType.CHARGE:
                checkbox.isOn = StaticInfo.chargeable;
                break;
            case UpgradeType.FREEZE:
                checkbox.isOn = StaticInfo.freezing;
                break;
            default:
                Debug.LogWarning("Invalid collectible type");
                break;
        }
    }

    void SetUpgrade(bool state){
        switch (type)
        {
            case UpgradeType.RETURN:
                StaticInfo.returning = state;
                break;
            case UpgradeType.CHARGE:
                StaticInfo.chargeable = state;
                break;
            case UpgradeType.FREEZE:
                StaticInfo.freezing = state;
                break;
            default:
                Debug.LogWarning("Invalid collectible type");
                break;
        }
    }
}
