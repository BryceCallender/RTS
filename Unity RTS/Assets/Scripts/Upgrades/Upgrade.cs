using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrades", order = 1)]
public class Upgrade : ScriptableObject
{
    public List<GameObject> unitsToApply;

    public int mineralRequirement;
    public int gasRequirement;

    public float researchTime;

}
