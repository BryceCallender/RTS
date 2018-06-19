using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;

public class AI : Player
{
    //What AI can make
    public List<GameObject> buildableUnits;
    public List<GameObject> buildableBuildings;

    public Dictionary<string, int> buildings = new Dictionary<string, int>();
    public Dictionary<string, int> units = new Dictionary<string, int>();

    //What AI has
    public List<GameObject> currentUnits;
    public List<GameObject> harvesters;
    public List<GameObject> currentBuildings;
    public Queue<UnitStruct> globalQueue;

    private List<GameObject> unitsToGrab;

    private static AI instance;

    public static AI Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

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

    public GameObject FindUnit(string name)
    {
        for (int i = 0; i < buildableUnits.Count; i++)
        {
            if(buildableUnits[i].gameObject.name.Contains(name))
            {
                return buildableUnits[i];
            }
        }
        return null;
    }
}
