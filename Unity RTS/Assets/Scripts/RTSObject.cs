using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    Red,
    Blue
}

public enum ArmorClass
{
    Light,
    Normal,
    Heavy,
    Bio,
    Air,
    Building
}

[RequireComponent(typeof(Health))]
public class RTSObject : MonoBehaviour
{
    public new string name;
    public Sprite uiSprite;

    [TextArea]
    public string unitDescription;

    [HideInInspector]
    public Health health;

    public int mineralCost;
    public int gasCount;

    public int damage;
    public float speed;
    public float range;

    public ArmorClass armorClass = ArmorClass.Normal;
    public float productionDuration;
    public Team team;

    public List<UpgradeData> upgrades;

    protected virtual void Awake()
    {
        health = GetComponent<Health>();
    }

    public void ApplyUpgrade(UpgradeData upgrade)
    {
        damage += upgrade.damageIncrease;
        range += upgrade.rangeIncrease;
        speed += upgrade.speedIncrease;
        armorClass += upgrade.armorIncrease;

        upgrades.Add(upgrade);
    }
}
