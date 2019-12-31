using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageHelper
{
    public static bool CanDamage(Team myTeam, Team otherTeam)
    {
        return myTeam != otherTeam;
    }

    public static float GetDamageModifier(ArmorClass armorClass, ArmorClass unitDamageStrength)
    {
        float damageModifier = 1.0f;

        return damageModifier;
    }

    public static bool IsUnitAbleToAttack(GameObject unit, GameObject enemy)
    {
        //Can attack anything
        if (unit.GetComponent<AttackInfo>().unitsUnitCanAttack == UnitsAttackable.All)
            return true;

        return unit.GetComponent<AttackInfo>().unitsUnitCanAttack == enemy.GetComponent<AttackInfo>().unitType;
    }
}
