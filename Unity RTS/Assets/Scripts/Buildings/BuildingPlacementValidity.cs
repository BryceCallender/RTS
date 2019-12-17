using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacementValidity : MonoBehaviour 
{
    public List<Collider> collisions = new List<Collider>();

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8 || other.gameObject.layer == 9
           || other.gameObject.layer == 11)
        {
            collisions.Add(other);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 9
           || other.gameObject.layer == 11)
        {
            collisions.Remove(other);
        }
    }
}
