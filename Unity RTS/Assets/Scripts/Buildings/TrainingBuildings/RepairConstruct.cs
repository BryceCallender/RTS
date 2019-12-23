using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairConstruct : TrainingBuilding
{
    private float repairRate = 1.0f;
    public Transform constructCenter;
    public Transform constructCrane;
    public ParticleSystem sparks;

    public List<Unit> unitsWithinConstructArea = new List<Unit>();

    private Camera camera;
    private RaycastHit hitInfo;

    [SerializeField]
    private bool repairMode;
    [SerializeField]
    private bool isRepairing;
    private Health repairingUnitHealth;

    protected override void Start()
    {
        base.Start();

        camera = Camera.main;
        sparks.Stop();
    }

    protected override void Update()
    {
        base.Update();

        //Check for repairs
        if(IsBuildingAvailableToUse())
        {
            if(UnitIsSelected)
            {
                if(Input.GetKeyDown(KeyCode.R))
                {
                    repairMode = !repairMode;
                }

                if (repairMode && Input.GetMouseButtonDown(1))
                {
                    Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                    int unitLayerMask = 1 << 8;
                    if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, unitLayerMask, QueryTriggerInteraction.Ignore))
                    {
                        if(CheckIfObjectHitIsInRepairRange(hitInfo.collider.gameObject))
                        {
                            Unit unit = hitInfo.collider.gameObject.GetComponent<Unit>();
                            if (!unit.GetComponent<Health>().isRepairable)
                            {
                                isRepairing = false;
                                return;
                            }

                            repairingUnitHealth = unit.GetComponent<Health>();
                            isRepairing = true;
                        }
                    }
                }

                if (isRepairing)
                {
                    AimCraneTowardsRepairingUnit();
                    RepairUnit();
                }
                else
                {
                    repairingUnitHealth = null;
                    sparks.Stop();
                }
            }
        }
    }


    private void RepairUnit()
    {
        if (!sparks.isPlaying)
            sparks.Play();
        
        if(repairingUnitHealth.currentHealth >= repairingUnitHealth.maxHealth)
        {
            isRepairing = false;
            return;
        }

        repairingUnitHealth.Repair(repairRate * Time.deltaTime);
    }

    private bool CheckIfObjectHitIsInRepairRange(GameObject gameObject)
    {
        if (gameObject.GetComponent<Unit>() == null)
            return false;

        return unitsWithinConstructArea.Contains(gameObject.GetComponent<Unit>());
    }

    private void AimCraneTowardsRepairingUnit()
    {
        Vector3 direction = repairingUnitHealth.gameObject.transform.position - constructCrane.position;
        Debug.DrawRay(constructCrane.position, direction, Color.cyan, 1.0f);
        direction.z = 0;
        constructCrane.rotation = Quaternion.Lerp(constructCrane.rotation, Quaternion.LookRotation(direction), Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        unitsWithinConstructArea.Add(other.gameObject.GetComponent<Unit>());
    }

    private void OnTriggerExit(Collider other)
    {
        unitsWithinConstructArea.Remove(other.gameObject.GetComponent<Unit>());
    }
}
