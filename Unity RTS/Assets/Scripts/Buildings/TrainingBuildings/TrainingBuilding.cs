using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingBuilding : Building
{
    public const int MAX_UNITS = 5;

    public List<Unit> producableUnits;
    public List<Unit> unitProductionList; //List acting like a queue (we can remove from anywhere in other RTS games)

    public Transform rallyPoint;

    public bool isProducingUnits;

    private void Update()
    {
        //Theres units to be produced!
        if(unitProductionList.Count > 0)
        {
            ProduceUnit();
        }
    }

    protected void AddUnitToQueue(Unit unit)
    {
        if (!producableUnits.Contains(unit))
        {
            Debug.LogError("Cant make this unit!");
        }


        if (unitProductionList.Count <= MAX_UNITS)
        {
            unitProductionList.Add(unit);

            //Take away unit price in money
        }
    }

    protected void RemoveUnitFromQueue(int position)
    {
        if (unitProductionList.Count > 0)
        {
            unitProductionList.RemoveAt(position);

            //Refund something back to the player
        }
    }

    protected void ProduceUnit()
    {

    }

    protected virtual void AnimateBuildingDuringProduction()
    {
        Debug.Log("Animate me!");
    }

    protected virtual void AnimateBuildingInProductionFinish()
    {
        Debug.Log("Animate me!");
    }



}
