using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UnitSelected))]
[RequireComponent(typeof(NavMeshObstacle))]
[RequireComponent(typeof(BoxCollider))]

public class Building : RTSObject, ISelectable
{
    public RequirementStructures requiredBuildingsToConstruct;
    protected UnitSelected unitSelected;
    protected bool UnitIsSelected => unitSelected.selected;

    [SerializeField]
    private float progress = 0.0f;
    [SerializeField]
    private bool isBuilding;
    [SerializeField]
    private bool alreadyPlaced;

    public Material constructionMaterial, finishedMaterial;
    [SerializeField]
    private MeshRenderer[] meshRenderers;
    private MaterialPropertyBlock propBlock;

    public readonly string buildProgressShaderName = "Vector1_79B66B06"; // weird naming thing unity chose

    private bool CompletedBuilding => progress >= productionDuration;

    // keep a copy of the executing script
    private Coroutine buildingCoroutine;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        unitSelected = GetComponent<UnitSelected>();
        unitSelected.enabled = false; //Dont enable selection until the building is available to the user

        propBlock = new MaterialPropertyBlock();

        //function normally
        buildingCoroutine = StartCoroutine(BuildBuilding());
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(IsBuildingAvailableToUse())
        {
            unitSelected.enabled = true;
            foreach (MeshRenderer renderer in meshRenderers)
            {
                renderer.material = finishedMaterial;
            }
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

        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.material = constructionMaterial;
        }

        while (isBuilding)
        {
            progress += Time.deltaTime;
            foreach(MeshRenderer renderer in meshRenderers)
            {
                //Property blocks ensures that the material changed isnt messing with the only reference to the material otherwise
                //everything would be loading with the same "progress" visually but not numerically :)
                renderer.GetPropertyBlock(propBlock);
                propBlock.SetFloat(buildProgressShaderName, progress.Remap(0, productionDuration, 0.3f, 0.85f));
                renderer.SetPropertyBlock(propBlock);
            }
            yield return null;
        }
    }

    protected bool IsBuildingAvailableToUse()
    {
        return CompletedBuilding || alreadyPlaced;
    }
}
