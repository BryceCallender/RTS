using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBuilding: MonoBehaviour
{
    public GameObject ghostBuilding; //For building placement
    public GameObject realBuilding;

    public Material validMaterial, invalidMaterial;

    private MeshRenderer[] meshRenderers;
    private Transform ghostBuildingTransform; //For tracking where it goes and its rotation
    
    private BuildingPlacementValidity placementOfBuildingValidity;

    public void CreateGhostBuilding()
    {
        ghostBuildingTransform = Instantiate(ghostBuilding).transform;
    }

    private void Start()
    {
        placementOfBuildingValidity = GetComponent<BuildingPlacementValidity>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        ghostBuildingTransform = ghostBuilding.transform;
    }

    private void Update()
    {
        MoveToMousePosition();

        //Make the ghost building red and dont allow it to be placed!
        if(!IsValidSpot())
        {
            foreach(MeshRenderer meshRenderer in meshRenderers)
                meshRenderer.material = invalidMaterial;
        }
        else
        {
            foreach (MeshRenderer meshRenderer in meshRenderers)
                meshRenderer.material = validMaterial;

            //Make it green incase it was red from an invalid spot!
            if (Input.GetMouseButtonDown(0))
            {
                PlaceBuilding();
            }
        }
    }

    private void PlaceBuilding()
    {
        //The placement is a valid location so place the building here

        //Delete the ghost building
        Destroy(ghostBuilding);

        //Instantiate the real deal here
        Instantiate(realBuilding, ghostBuildingTransform.position, ghostBuildingTransform.rotation);
    }

    
    private bool IsValidSpot()
    {
        return placementOfBuildingValidity.collisions.Count == 0;
    }

    private void MoveToMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            ghostBuildingTransform.position = new Vector3(hitInfo.point.x, 0, hitInfo.point.z);
        }
    }
}
