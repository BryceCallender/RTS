using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public int resourceLeft = 100;

    private void Update()
    {
        if(resourceLeft <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
       Destroy(gameObject);
    }
}
