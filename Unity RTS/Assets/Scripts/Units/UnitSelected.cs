using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelected : MonoBehaviour 
{
    public Vector2 ScreenPos;
    public bool selected;
    public bool added;
	public bool isFirst;
    public Mouse mouse;

    public bool disable;

    public GameObject selectionIndicator;
    private RaycastHit hitInfo;
    private Camera camera;

    public float indicatorResizing = 1.25f;

    //The selection indicator is not going to be enabled so we have to use awake since itll run regardless of the script being
    //enabled or not unlike Start
    private void Awake()
    {
        mouse = FindObjectOfType<Mouse>();
        camera = Camera.main;
        selectionIndicator = transform.Find("SelectionIndicator").gameObject;
    }

    private void Update()
    {
        if (disable)
        {
            enabled = false;
            return;
        }

        ScreenPos = camera.WorldToScreenPoint(transform.position);
        if (mouse.UnitInsideScreen(ScreenPos))
        {
            //Responsible for adding units on a mouse drag
            if (mouse.UnitInDragBox(ScreenPos) && !added && !selected && Mouse.IsDragging)
            {
                mouse.unitsOnScreen.Add(gameObject);
                selected = true;
                added = true;

                CalculateBounds();
            }
            //Responsible for adding a single unit based on who we click!
            else if (Input.GetMouseButtonDown(0) && !added && !selected && !Mouse.IsDragging && !Mouse.ShiftKeyDown())
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
                {
                    //Make sure that we pick the object we hit and not everything since every object has this script on them
                    //Also ensure that the object we hit has the Selectable interface
                    if (hitInfo.collider.gameObject.name.Equals(gameObject.name) && hitInfo.collider.gameObject.GetInterface<ISelectable>() != null)
                    {
                        mouse.DeselectAllUnits();
                        mouse.unitsOnScreen.Add(hitInfo.collider.gameObject);
                        mouse.selectedObjects.Add(hitInfo.collider.gameObject);
                        selected = true;
                        added = true;

                        CalculateBounds();
                    }
                }
            }
        }

        if(Mouse.ShiftKeyDown() && Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                //If object implements ISelectable then remove it from the list
                if(hitInfo.collider.gameObject.name.Equals(gameObject.name) && hitInfo.collider.gameObject.GetInterface<ISelectable>() != null)
                {
                    UnitSelected unitHit = hitInfo.collider.gameObject.GetComponent<UnitSelected>();

                    Debug.Log($"You dun did a shift click on : {hitInfo.collider.gameObject}");

                    if (unitHit.selected)
                    {
                        Debug.Log("Removed one");
                        mouse.DeselectShiftedUnit(hitInfo.collider.gameObject);
                    }
                    else
                    {
                        Debug.Log("Added one");
                        mouse.AddShiftedUnit(hitInfo.collider.gameObject);
                    }

                    unitHit.selected = !unitHit.selected;
                    unitHit.added = !unitHit.added;
                    unitHit.selectionIndicator.SetActive(!unitHit.selectionIndicator.activeSelf);

                    if(unitHit.selectionIndicator.activeSelf)
                    {
                        CalculateBounds();
                    }
                    
                }
                //else 
                //{
                //    Debug.Log("Removed all");
                //    mouse.DeselectAllUnits();
                //    selected = false;
                //    added = false;
                //    selectionIndicator.SetActive(false);
                //}
            }	
        }

		isFirst = mouse.IsFirstInList(gameObject);
    }

    private void CalculateBounds()
    {
        Bounds bigBounds = gameObject.GetComponentInChildren<Renderer>().bounds;

        // This "diameter" only works correctly for relatively circular or square objects
        float diameter = bigBounds.size.z;
        diameter *= indicatorResizing;

        selectionIndicator.SetActive(true);

        selectionIndicator.transform.position = new Vector3(bigBounds.center.x, 0f, bigBounds.center.z);
        selectionIndicator.transform.localScale = new Vector3(bigBounds.size.x + (diameter / 2.0f), bigBounds.size.y, bigBounds.size.z + (diameter / 2.0f));
    }
}
