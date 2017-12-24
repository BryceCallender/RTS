using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour 
{
    public int currency;
    public int capacityMax;
    public int currentCapacity;
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
