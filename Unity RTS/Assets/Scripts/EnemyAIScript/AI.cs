using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour 
{
    //Basic money, max capacity, and current capacity
    public int currency;
    public int capacityMax;
    public int currentCapacity;

    //What AI can make
    public List<GameObject> buildableUnits;
    public List<GameObject> buildableBuildings;

    //What AI has
    public List<GameObject> currentUnits;
    public List<GameObject> currentBuildings;
    private List<GameObject> unitsToGrab;

	// Use this for initialization
	void Start () 
    {
        capacityMax = 50;
        currency = 0;
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}
}
