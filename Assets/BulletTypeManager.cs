using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletTypeManager : MonoBehaviour {

	public static bool returning = false;
    public static bool chargeable = false;
    public static bool freezing = false;

    int activeAbility;

	void Start(){
        Switch(0);
    }

    void Update()
    {
        if (StaticInfo.stackableAbilities)
        {
            returning = StaticInfo.returning;
            chargeable = StaticInfo.chargeable;
            freezing = StaticInfo.freezing;
        }
        else
        {
            if (Input.GetButtonDown("SwitchBullet"))
            {
                Switch(1);
            }
			if(Input.GetAxisRaw("MouseWheel") != 0){
                Switch(Math.Sign(Input.GetAxisRaw("MouseWheel")));
            }
            if (Input.GetButtonDown("ChooseReturn"))
            {
                Select(UpgradeType.RETURN);
            }
            if (Input.GetButtonDown("ChooseCharge"))
            {
                Select(UpgradeType.CHARGE);
            }
            if (Input.GetButtonDown("ChooseFreeze"))
            {
                Select(UpgradeType.FREEZE);
            }
        }
    }

    public void Switch(int offset)
    {
        int possibilities = 1 + (StaticInfo.returning ? 1 : 0) + (StaticInfo.chargeable ? 1 : 0) + (StaticInfo.freezing ? 1 : 0);
        activeAbility = ((activeAbility + offset) % possibilities + possibilities) % possibilities;

		returning = chargeable = freezing = false;
        switch (activeAbility)
        {
			//case 0 is no upgrades
			case 1:
				if(StaticInfo.returning) returning = true;
				else if(StaticInfo.chargeable) chargeable = true;
				else if(StaticInfo.freezing) freezing = true;
                break;
			
			case 2:
				if (StaticInfo.returning && StaticInfo.chargeable) chargeable = true;
                else freezing = true;
                break;
			
			case 3:
                freezing = true;
                break;
        }
	}

	public void Select(UpgradeType type){
		returning = chargeable = freezing = false;
		switch(type){
			case UpgradeType.RETURN:
                if (StaticInfo.returning)
                {
                    returning = true;
                    activeAbility = 1;
                }
                break;
            case UpgradeType.CHARGE:
				if(StaticInfo.chargeable)
				{
                    chargeable = true;
                    activeAbility = 1 + (StaticInfo.returning ? 1 : 0);
                }
				break;
			case UpgradeType.FREEZE:
				if (StaticInfo.freezing)
                {
                    freezing = true;
                    activeAbility = 1 + (StaticInfo.returning ? 1 : 0) + (StaticInfo.chargeable ? 1 : 0);
                }
                break;
        }
	}

	public static bool ActiveBulletState(UpgradeType type)
    {
		switch (type)
        {
            case UpgradeType.RETURN:
                return returning;
            case UpgradeType.CHARGE:
				return chargeable;
            case UpgradeType.FREEZE:
				return freezing;
			default:
                return false;
        }
    }
}

public enum UpgradeType
{
    RETURN,
    CHARGE,
    FREEZE
}