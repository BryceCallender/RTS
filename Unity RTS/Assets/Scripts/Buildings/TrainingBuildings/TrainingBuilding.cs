using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RTSLineRenderer))]
public class TrainingBuilding : Building
{
    public const int MAX_UNITS = 5;

    public List<Unit> producableUnits;
    public List<Unit> unitProductionList; //List acting like a queue (we can remove from anywhere in other RTS games)
    public Transform spawnPosition;

    public Vector3 rallyPoint;
    private RTSLineRenderer rtsLineRenderer;

    private Camera camera;
    private RaycastHit hitInfo;

    public bool isProducingUnits;

    private float unitProductionTimer = 0.0f;
    [SerializeField]
    private Unit currentUnit;

    private static int number = 1;

    protected override void Start()
    {
        base.Start();
        camera = Camera.main;
        rtsLineRenderer = GetComponent<RTSLineRenderer>();

        rtsLineRenderer.AddLinePoint(transform.position); //Home point!
    }

    protected override void Update()
    {
        base.Update();

        if(IsBuildingAvailableToUse())
        {
            if(UnitIsSelected)
            {
                rtsLineRenderer.Show();
            }
            else
            {
                rtsLineRenderer.Hide();
            }
            

            if (UnitIsSelected && Input.GetKeyDown(KeyCode.F5))
            {
                unitProductionList.Add(producableUnits[Random.Range(0, producableUnits.Count)]);
            }

            //Theres units to be produced!
            if (unitProductionList.Count > 0)
            {
                currentUnit = unitProductionList[0];
                ProduceUnit();
            }
            else
            {
                isProducingUnits = false;
            }

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                if (Input.GetMouseButtonDown(1) && UnitIsSelected)
                {
                    if (hitInfo.collider.name.Equals("RTSTerrain") || hitInfo.collider.gameObject.layer == 8 || hitInfo.collider.gameObject.layer == 9)
                    {
                        SetRallyPoint(hitInfo.point);
                    }
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
            //Now that the current unit has been dismissed reset the timer
            //other positions will not affect the timer at any point other 
            //than removing the currently worked one.
            if(position == 0 && unitProductionTimer >= currentUnit.productionDuration)
            {
                GameObject newUnit = Instantiate(UnitManager.GetGameObjectFromUnit(currentUnit), spawnPosition.position, spawnPosition.rotation);

                //TODO::Fix numbering system
                newUnit.name = currentUnit.gameObject.name + "_" + number;
                number++;

                //Zero means rally point has not been defined
                if (rallyPoint == Vector3.zero)
                {
                    newUnit.GetComponent<Unit>().agent.destination = spawnPosition.position + new Vector3(0, 0, 2);
                }
                else
                {
                    newUnit.GetComponent<Unit>().agent.destination = rallyPoint;
                }

                unitProductionTimer = 0;
            }
            else
            {
                //Refund something back to the player
            }

            unitProductionList.RemoveAt(position);
        }
    }

    protected void ProduceUnit()
    {
        if(unitProductionTimer >= currentUnit.productionDuration)
        {
            AnimateBuildingInProductionFinish();
            RemoveUnitFromQueue(0);
        }
        isProducingUnits = true;
        unitProductionTimer += Time.deltaTime;
    }

    protected virtual void DisplayUIElements()
    {
        Debug.Log("Tell me how to interact with the UI!");
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

        if(rtsLineRenderer.GetPointCount() < 2)
        {
            rtsLineRenderer.AddLinePoint(location);
        }
        else
        {
            rtsLineRenderer.UpdatePointPosition(1, location);
        }
    }


}
