using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BuildingPlacementValidity))]
public class GhostBuilding: MonoBehaviour
{
    public GameObject ghostBuilding; //For building placement
    public GameObject realBuilding;

    public Material validMaterial, invalidMaterial;

    private MeshRenderer[] meshRenderers;
    private Transform ghostBuildingTransform; //For tracking where it goes and its rotation
    
    private BuildingPlacementValidity placementOfBuildingValidity;

    private void Start()
    {
        placementOfBuildingValidity = GetComponent<BuildingPlacementValidity>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        ghostBuildingTransform = ghostBuilding.transform;
    }

    private void Update()
    {
        MoveToMousePosition();
       
        if(Input.GetKeyDown(KeyCode.R))
        {
            Rotate();
        }

        //Make the ghost building red and dont allow it to be placed!
        if(!IsValidSpot())
        {
            foreach(MeshRenderer meshRenderer in meshRenderers)
                meshRenderer.material = invalidMaterial;

            if(Input.GetMouseButtonDown(0))
            {
                NotificationSystem.NotifyMessage("You cannot place a building there!");
            }
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

    public void CreateGhostBuilding()
    {
        ghostBuildingTransform = Instantiate(ghostBuilding).transform;
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
        if(placementOfBuildingValidity.RequiredStructureToBePlacedOn == null)
        {
            return placementOfBuildingValidity.collisions.Count == 0;
        }
        else
        {
            return placementOfBuildingValidity.IsAboveRequiredStructure() && placementOfBuildingValidity.collisions.Count == 1;
        }
    }

    private void MoveToMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        int layersToHit = ~LayerMask.GetMask("GhostBuilding"); //Dont need to check for collisions of a ghostbuilding since itll proc everytime if it did

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layersToHit))
        {
            if(placementOfBuildingValidity.RequiredStructureToBePlacedOn != null && 
                hitInfo.collider.gameObject.name.Equals(placementOfBuildingValidity.RequiredStructureToBePlacedOn.name))
            {
                ghostBuildingTransform.position = hitInfo.collider.gameObject.transform.position;
            }
            else
            {
                ghostBuildingTransform.position = new Vector3(hitInfo.point.x, 0, hitInfo.point.z);
            }
            
        }
    }

    private void Rotate()
    {
        ghostBuildingTransform.Rotate(Vector3.up, 90);
    }
}
