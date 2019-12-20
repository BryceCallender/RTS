using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingBuilding : Building
{
    public const int MAX_UNITS = 5;

    public List<Unit> producableUnits;
    public List<Unit> unitProductionList; //List acting like a queue (we can remove from anywhere in other RTS games)

    public Vector3 rallyPoint;
    private Camera camera;
    private RaycastHit hitInfo;

    public bool isProducingUnits;

    protected override void Start()
    {
        base.Start();

        camera = Camera.main;
    }

    protected override void Update()
    {
        base.Update();

        //Theres units to be produced!
        if (unitProductionList.Count > 0)
        {
            ProduceUnit();
        }

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            if(Input.GetMouseButtonDown(1))
            {
                if (hitInfo.collider.name.Equals("RTSTerrain") || hitInfo.collider.gameObject.layer == 8 || hitInfo.collider.gameObject.layer == 9)
                {
                    SetRallyPoint(hitInfo.point);
                }
            }
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


    public void SetRallyPoint(Vector3 location)
    {
        Debug.Log($"Rally point set at {location}");
        location.y = 0;
        rallyPoint = location;
    }


}
