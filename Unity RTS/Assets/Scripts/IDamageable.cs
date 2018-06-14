using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public interface IDamageable
{
    bool TakeDamage(float damage, IAlignmentProvider damageAlignment);
    void Die();
}
