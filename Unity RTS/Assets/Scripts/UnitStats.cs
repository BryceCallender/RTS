using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitStats : MonoBehaviour
{
    //TODO: Make enum and assign a 0 or 1 for friendly or not (Idea for now)
    public abstract void Die();
    public abstract void TakeDamage(int damage);
    public abstract GameObject FindEnemy();
    public abstract void Fire();

}
