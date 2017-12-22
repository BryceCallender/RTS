using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterAI : MonoBehaviour 
{
    public int health = 30;
    public int resourceAmount = 0;
    public int maxResourceToCollect = 100;

    public Resource[] resources;
    public Resource nearestResource;
    public Transform resourceCollector;

    public bool isTargetingResource = false;
    public bool isFull = false;
    public bool isTurnedIn = false;
    public bool isResourceGone = false;
    public bool foundResource = true;

    Animator anim;

    public float timer = 0;
    public float timeToWait = 1.0f;

    public float harvestTime = 2.0f;
    public float harvestCoolDown = 3.0f;
    public int harvestAmount = 10;

	// Use this for initialization
	void Start () 
    {
        anim = GetComponent<Animator>();
        resources = FindObjectsOfType<Resource>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        FindResource();
        anim.SetBool("foundResource",true);
	}

    public void FindResource()
    {
        float distance = Mathf.Infinity;

        foreach(Resource resource in resources)
        {
            if(resource != null)             {                 float resourceDistance = Vector3.Distance(this.transform.position, resource.transform.position);                 if (nearestResource == null || resourceDistance < distance)
                {                     nearestResource = resource;                     distance = resourceDistance;                 }             }         }          if (nearestResource == null)         {             return;          }  
        }
}
