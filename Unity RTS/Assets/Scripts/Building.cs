using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : RTSObject
{
    public GameObject ghostBuilding; //For building placement
    
    public RequirementStructures requiredBuildingsToConstruct;

    public List<Unit> unitsCanMake;
    public Queue<Unit> unitQueue;

    private void placeBuilding()
    {
        
    }
}
