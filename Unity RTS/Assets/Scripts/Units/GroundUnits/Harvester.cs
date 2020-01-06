using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Harvester : Unit
{
    [Header("Harvester Stats")]
    public int resourceAmount = 0;
    public int maxResourceToCollect = 100;
    
    [Header("Harvester Building List")]
    //Harvester Building Capabilites and what building to build if we want one
    public List<Building> buildableBuildings;
    private GameObject buildingToBuild;
    
    // Resources and collector for it to go to 
    [Header("Harvester Resource/Collector")]
    public List<Resource> resources;
    public Resource nearestResource;
    public Transform closestResourceCollector;
    public List<GameObject> resourceCollectors;
    
    //Harvester Abilities Harvest amount and cooldowns
    private float harvestTime = 2.0f;
    private float harvestCoolDown = 3.0f;
    private int harvestAmount = 10;
    [SerializeField]
    private GameObject crystal;
    
    //Booleans
    private bool isTargetingResource;
    private bool isFull;
    private bool isTurnedIn;
    private bool isAtResource;
    private bool isMining;

    //Timers
    private float timer = 0;
    private float timeToWait = 1.0f;

    private void Start()
    {
        base.Start();
        
        crystal.gameObject.SetActive(false);
        
        //Adds all the supply buildings into the list of resource collectors
        //The enemy ai will have a different tag to differeniate between theirs and ours
        //TODO:: This will be slow we will need to fix this if there is a lot of supply buildings
        //present in the game
        resourceCollectors.AddRange(GameObject.FindGameObjectsWithTag("SupplyBuilding"));

        FindClosestResourceCollector();
        
        //Find all the resources on the map when they start living
        resources.AddRange(FindObjectsOfType<Resource>());
    }

    protected override void Update()
    {
        resources.RemoveAll(resource => resource == null);
        
        //Automatic Behavior
        if(isMining)
        {
            FindResource();
        }

        //Manual Behavior
		//We have found a resource
        if(nearestResource != null)
        {
            Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray2, out hitInfo, Mathf.Infinity))
            {
                WhereToGo();
            }

            if (agent.remainingDistance <= agent.stoppingDistance && !isFull && isAtResource)
            {
                CollectResource();
            }

            if (resourceAmount >= maxResourceToCollect || nearestResource.resourceLeft == 0)
			{
                isFull = true;
                crystal.gameObject.SetActive(true);
                
                if(closestResourceCollector != null)
                {
                    agent.destination = closestResourceCollector.position;
                }

                timer += Time.deltaTime;

                if (agent.remainingDistance <= agent.stoppingDistance && !isTurnedIn && timer >= timeToWait)
                {
                    if(closestResourceCollector != null)
                    {
                        isTurnedIn = true;
                        TurnInResource();
                        timer = 0;
                    }
                }
                isTurnedIn = false;
                isFull = false;
			}

        }
		//Need to find a resource
        else
        {
            //raycast to check where user clicked mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //TODO: Need to make harvester more effective with moving 
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                //If right clicked and unit is selected go to resource
                WhereToGo();
                if (nearestResource == null && resourceAmount > 0)
                {
                    if(closestResourceCollector != null)
                    {
                        agent.destination = closestResourceCollector.position;
                    }

                    timer += Time.deltaTime;
                    if (agent.remainingDistance <= agent.stoppingDistance && !isTurnedIn && timer >= timeToWait)
                    {
                        Debug.Log("We should turn in the stoff");
                        TurnInResource();
                        timer = 0;
                        isTurnedIn = true;
                    }
                    isTurnedIn = false;
                    isFull = false;
                }
            }
        }
	}

    /*
     * This function makes the harvester wait a bit to collect each resource 
     * and as it collects it takes from the resources resovoir 
     */
    public void CollectResource()
    {
        harvestCoolDown -= Time.deltaTime;
        if(harvestCoolDown < 0)
        {
            harvestCoolDown = harvestTime;
            resourceAmount += harvestAmount;
            nearestResource.resourceLeft -= harvestAmount;
        }
    }

	/*
     * This function makes the harvester turn in what it has and adds 
     * to the currency of the user 
     */
	public void TurnInResource()
    {
        closestResourceCollector.GetComponent<SupplyBuilding>().CollectFromHarvester(resourceAmount);
        resourceAmount = 0;
        crystal.gameObject.SetActive(false);
    }

	/*
     * This function looks at all the resources on map and picks closest one
     */
	public void FindResource()
    {
        float distance = Mathf.Infinity;

        foreach (Resource resource in resources)
        {
            if(resource != null)
            {
				float resourceDistance = Vector3.Distance(transform.position, resource.transform.position);
				if (nearestResource == null || resourceDistance < distance)
				{
					nearestResource = resource;
					distance = resourceDistance;
				}
            }

        }

        if (nearestResource == null)
        {
            return; 
        }
    }

    /*
     * Tells the harvester where to go based on if it is selected and if 
     * the user has right clicked down on  the mouse. If it hits a resource the
     * harvester will go to the resource and start collecting and if its just
     * a random point on the map the harvester will go to that specific point
     */
    public void WhereToGo()
    {
		if (Input.GetMouseButtonDown(1) && unitSelected.selected)
		{
			if (hitInfo.transform.gameObject.CompareTag("Resource"))
			{
				GameObject resource = hitInfo.transform.gameObject;
				nearestResource = resource.GetComponent<Resource>();
				agent.destination = nearestResource.transform.position;
                isAtResource = true;
			}
			else
			{
				agent.destination = hitInfo.point;
                isAtResource = false;
			}
		}
    }

    private void FindClosestResourceCollector()
    {
        float distance = float.PositiveInfinity;
        foreach (GameObject supplyBuilding in resourceCollectors)
        {
            if (distance > Vector3.Distance(transform.position, supplyBuilding.transform.position))
            {
                distance = Vector3.Distance(transform.position, supplyBuilding.transform.position);
                closestResourceCollector = supplyBuilding.transform;
            }
        }
    }
}
