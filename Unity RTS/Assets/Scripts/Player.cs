using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int currency;
    public int currentCapacity;
    public readonly int MAX_CAPACITY = 100;

    public HashSet<Unit> availableUnits;
    public HashSet<Building> availableBuildings;

    [SerializeField]
    private TextMeshProUGUI capacityText;

    private void Start()
    {
        currentCapacity = 0;
        capacityText.SetText(currentCapacity + "/" + MAX_CAPACITY);

        availableUnits = new HashSet<Unit>();
        availableBuildings = new HashSet<Building>();
    }

    public bool CheckIfPlayerCanBuildStructure(Building building)
    {
        //Go through all the buildings required in the building passed in
        foreach(Building buildingToCheck in building.requiredBuildingsToConstruct.requiredBuildings)
        {
            //Go through our current buildings available to use
            foreach(Building buildingAvailable in availableBuildings)
            {
                //If we dont have it then return false indicating building is to be blocked still
                if(!availableBuildings.Contains(buildingToCheck))
                {
                    return false;
                }
            }
        }

        return true;
    }

    //TODO::Units can probably have required buildings. Implement that later
    public bool CheckIfPlayerCanBuildUnit(Unit unit)
    {
        return true;
    }
}
