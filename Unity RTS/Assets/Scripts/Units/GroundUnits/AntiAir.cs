using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MissileLauncher
{
    public GameObject missileHead;
    public bool isAvailable;
}

public struct MissileInfo
{
    public Transform missileTransform;
    public int missileIndex;
}

public class AntiAir : MonoBehaviour
{
    [HideInInspector]
    public float missileReloadTime;
    private const int MISSILE_COUNT = 3;

    public MissileLauncher[] missiles = new MissileLauncher[MISSILE_COUNT];

    public MissileInfo FindAvailableMissileToLaunch()
    {
        bool available = false;
        int missileIndex = 0;
        MissileInfo missileInfo;

        if (!AreMissilesAvailable())
        {
            missileInfo.missileIndex = -1;
            missileInfo.missileTransform = null;
            return missileInfo;
        }
             

        //Loop until a missile is available
        while (!available)
        {
            missileIndex = Random.Range(0, MISSILE_COUNT);
            available = missiles[missileIndex].isAvailable;
        }

        missileInfo.missileTransform = missiles[missileIndex].missileHead.transform;
        missileInfo.missileIndex = missileIndex;

        return missileInfo;
    }

    public bool AreMissilesAvailable()
    {
        int unavailableMissiles = 0;

        for (int i = 0; i < MISSILE_COUNT; i++)
        {
            if (!missiles[i].isAvailable)
            {
                unavailableMissiles++;
            }
        }

        //Missiles are available if the number of unavilable are less than missile count
        return unavailableMissiles < MISSILE_COUNT;
    }

    public void SetMissileReloadTime(float fireRate)
    {
        missileReloadTime = fireRate + 0.5f;
    }

    public void EnableMissile(int missileIndex, bool enabled)
    {
        //Make the missile be shot!
        missiles[missileIndex].isAvailable = enabled;
        missiles[missileIndex].missileHead.SetActive(enabled);
    }

    public IEnumerator ReloadMissileHead(int missileIndex)
    {
        yield return new WaitForSeconds(missileReloadTime);
        EnableMissile(missileIndex, true);
    }
}
