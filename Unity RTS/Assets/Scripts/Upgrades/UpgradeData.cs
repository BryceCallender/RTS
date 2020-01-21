using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrades", order = 1)]
public class UpgradeData : ScriptableObject
{
    public Sprite upgradeSprite;

    public List<GameObject> unitsToApply;

    public int mineralRequirement;
    public int gasRequirement;

    public float researchTime;

    public int damageIncrease;
    public int armorIncrease;
    public int rangeIncrease;
    public int speedIncrease;
}
