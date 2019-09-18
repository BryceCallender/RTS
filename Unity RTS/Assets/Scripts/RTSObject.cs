using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    Flyer
}

public class RTSObject : MonoBehaviour
{
    public string name;
    public Sprite uiSprite;
    
    public int health;
    public int cost;
    public float speed;
    public float range;
    
    public ArmorClass armorClass;
    public float productionDuration;
    public Team team;

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    public static bool CanDamage(Team myTeam, Team otherTeam)
    {
        return myTeam != otherTeam;
    }
}
