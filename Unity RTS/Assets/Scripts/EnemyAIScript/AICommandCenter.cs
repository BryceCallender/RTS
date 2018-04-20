using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICommandCenter : MonoBehaviour
{
    public float health;
    public AI AIManager;

    public int buildingCount;
    public bool isOkToPlace = false;

    [SerializeField]
    private GameObject chosenBuilding;

    private PlaceableBuilding placeBuilding;

    private const int MAX_RADIUS = 9000;
    private int radiusOfInfluence = 100;

    private int timesBuildResource = 1;

    Vector3 centerOfBuilding;
    Vector3 sizeOfBound;

    // Use this for initialization
    void Start()
    {
        health = 1000;
        buildingCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (CheckIfBuildingCanBePlaced(GetRandomLocation(gameObject.transform)))
        {
            //Instantiate(chosenBuilding, centerOfBuilding,Quaternion.Euler(new Vector3(0,-180,0)));
            //if (radiusOfInfluence <= MAX_RADIUS)
            //{
            //    radiusOfInfluence += 100;
            //}
        }
    }

    //TODO::We need to make it build close to itself
    //or the base building will be sparatic and truly random 
    //want to give it a sense of smart
    public Vector3 GetRandomLocation(Transform commandLocation)
    {
        Vector3 location;
        //We want random spot but we want it on the ground so dont change 
        //the y value only just the x and z values
        location.x = Random.Range(-radiusOfInfluence, radiusOfInfluence) + commandLocation.position.x;
        location.y = commandLocation.position.y;
        location.z = Random.Range(-radiusOfInfluence, radiusOfInfluence) + commandLocation.position.z;

        return location;
    }

    public bool CheckIfBuildingCanBePlaced(Vector3 placeToCheck)
    {
        //chosenBuilding = BuildingToBuild();

        BoxCollider bound = chosenBuilding.GetComponent<BoxCollider>();

        var layerMask = 1 << 13;
        layerMask = ~layerMask;

        sizeOfBound = bound.size;

        centerOfBuilding = placeToCheck;

        return !Physics.CheckBox(centerOfBuilding, sizeOfBound / 2, Quaternion.identity,layerMask);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radiusOfInfluence);
        //Gizmos.DrawLine(gameObject.transform.position,GetRandomLocation(gameObject.transform));
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(centerOfBuilding,sizeOfBound);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(GetRandomLocation(gameObject.transform),Vector3.one*10);
    }

    public GameObject BuildingToBuild()
    {
        GameObject building = null;

        //If we have full capacity ATM and can build more houses 
        //or if the current capacity plus the ones we are building then
        //lets make a house 
        if (AIManager.currentCapacity == AIManager.capacityMax
            //(AIManager.currentCapacity + AIManager.globalQueue.Count) > AIManager.capacityMax
            && GameController.MAX_CAPACITY != AIManager.capacityMax ||
            //Or the queue is currently over the max so lets quickly make housing
            AIManager.currentCapacity + GetUnitCount() > AIManager.capacityMax)
        {
            //house building
            if (AIManager.currency >= AIManager.buildings["House"])
            {
                //build house
            }
        }

        //Builds every increment of 10 harvesters to make it somewhat
        //efficient
        if (AIManager.harvesters.Count > 10 * timesBuildResource)
        {
            //Resource building
            if (AIManager.currency >= AIManager.buildings["Supply"])
            {
                //build resource
                timesBuildResource++;
            }
        }

        return building;
    }

    public GameObject FindHarvesterToBuildBuilding()
    {
        GameObject harvester;

        int RNG = Random.Range(0,AIManager.harvesters.Count);
        harvester = AIManager.harvesters[RNG];

        return harvester;
    }

    public int GetUnitCount()
    {
        return AIManager.globalQueue.Count;
    }
}

