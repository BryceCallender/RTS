using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int currency;
    public int currentCapacity;
    public readonly int MAX_CAPACITY = 100;

    public List<Unit> units;
    public List<Building> buildings;

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

        units = new List<Unit>();
        buildings = new List<Building>();
    }

    public bool CheckIfPlayerCanBuildStructure(Building building)
    {
        //Go through all the buildings required in the building passed in
        foreach(Building buildingToCheck in building.requiredBuildingsToConstruct.requiredBuildings)
        {
            //If we dont have it then return false indicating building is to be blocked still
            if(!availableBuildings.Contains(buildingToCheck))
            {
                return false;
            }
        }

        return true;
    }

    //TODO::Units can probably have required buildings. Implement that later
    public bool CheckIfPlayerCanBuildUnit(Unit unit)
    {
        return true;
    }

    public void AddUnit(Unit unit)
    {
        units.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        units.Remove(unit);
    }

    public void AddBuilding(Building building)
    {
        buildings.Add(building);
    }

    public void RemoveBuilding(Building building)
    {
        buildings.Remove(building);
    }

    public void ChangeCapacity(int capacityChange)
    {
        currentCapacity += capacityChange;

        if(currentCapacity >= MAX_CAPACITY)
        {
            currentCapacity = MAX_CAPACITY;
        }
    }

}
