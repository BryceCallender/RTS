using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public UpgradeData upgrade;

    private void Start()
    {
        //Apply the upgrade to the unit
        gameObject.GetComponent<RTSObject>().ApplyUpgrade(upgrade);
    }
}
