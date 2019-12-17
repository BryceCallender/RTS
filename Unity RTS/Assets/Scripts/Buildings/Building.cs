using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : RTSObject
{
    public RequirementStructures requiredBuildingsToConstruct;

    [SerializeField]
    private float progress = 0.0f;
    [SerializeField]
    private bool isBuilding;
    private bool CompletedBuilding => progress >= productionDuration;

    // keep a copy of the executing script
    private Coroutine buildingCoroutine;

    // Start is called before the first frame update
    protected void Start()
    {
        //Buildings cant move and cant shoot with their own armor class defaulted
        //incase the values arent correctly set
        speed = 0;
        range = 0;
        armorClass = ArmorClass.Building;

        //function normally
        buildingCoroutine = StartCoroutine(BuildBuilding());
    }

    // Update is called once per frame
    protected void Update()
    {
        if(CompletedBuilding)
        { 
            StopBuilding();
        }
    }

    private void CancelBuilding()
    {
        isBuilding = false;
        progress = 0.0f;

        //Delete the building prefab
        Destroy(gameObject);

        //return 75% of the profit back to the user
    }

    private void StopBuilding()
    {
        isBuilding = false;
        StopCoroutine(buildingCoroutine);
    }

    private void ResumeBuilding()
    {
        buildingCoroutine = StartCoroutine(BuildBuilding());
    }

    private IEnumerator BuildBuilding()
    {
        isBuilding = true;
        while(isBuilding)
        {
            progress += Time.deltaTime;
            yield return null;
        }
    }
}
