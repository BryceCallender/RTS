using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICommandCenter : MonoBehaviour 
{
    public float health;
    public AI AIManager;

    public int buildingCount;
    public bool isOkToPlace = false;

    private GameObject chosenBuilding;

    private BoxCollider buildingBoxCollider;
    private 

    private const int MAX_RADIUS = 1000;
    private int radiusOfInfluence = 100;

	// Use this for initialization
	void Start () 
    {
        health = 1000;
        buildingCount = 0;
	}
	
	// Update is called once per frame
	void Update () 
    {
        //if(CheckIfBuildingCanBePlaced(GetRandomLocation(gameObject.transform)))
        //{
            
        //}
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
        chosenBuilding = BuildingToBuild();
        return true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, radiusOfInfluence);
        //Gizmos.DrawLine(gameObject.transform.position,GetRandomLocation(gameObject.transform));
    }

    public GameObject BuildingToBuild()
    {
        return null;
    }
}
