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
    public List<Resource> resourceList;

    public Animator anim;

	// Use this for initialization
	void Start () 
    {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        //TODO::fix this. This is gross making new lists everytime maybe dont 
        //need it since all resources will be laid out in game??? if so 
        //place back in start and no more worrying.
        resources = FindObjectsOfType<Resource>();
        resourceList = new List<Resource>(resources);

        FindResource();
        if(resourceList.Count > 0)
            anim.SetBool("foundResource",true);
	}

    public void FindResource()
    {
        float distance = Mathf.Infinity;

        foreach(Resource resource in resources)
        {
            if(resource != null)             {                 float resourceDistance = Vector3.Distance(this.transform.position, resource.transform.position);                 if (nearestResource == null || resourceDistance < distance)
                {                     nearestResource = resource;                     distance = resourceDistance;                 }             }         }          if (nearestResource == null)
            return;
        }
}
