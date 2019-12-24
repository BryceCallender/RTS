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
}
