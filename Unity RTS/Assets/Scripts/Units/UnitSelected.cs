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

    private GameObject selectionIndicator;
    private RaycastHit hitInfo;
    private Camera camera;

    private void Start()
    {
        selectionIndicator = transform.Find("SelectionIndicator").gameObject;
        mouse = FindObjectOfType<Mouse>();
        camera = Camera.main;
    }

    private void Update()
    {
        ScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        if (mouse.UnitInsideScreen(ScreenPos))
        {
            if (mouse.UnitInDragBox(ScreenPos) && !added && !selected && Mouse.IsDragging)
            {
                mouse.unitsOnScreen.Add(gameObject);
                selected = true;
                added = true;
				Bounds bigBounds = gameObject.GetComponentInChildren<Renderer>().bounds;

				// This "diameter" only works correctly for relatively circular or square objects
				float diameter = bigBounds.size.z;
				diameter *= 1.10f;

                selectionIndicator.SetActive(true);

                selectionIndicator.transform.position = new Vector3(bigBounds.center.x, 0.06f, bigBounds.center.z);
                selectionIndicator.transform.localScale = new Vector3(bigBounds.size.x + (diameter / 2.0f), bigBounds.size.y, bigBounds.size.z + (diameter / 2.0f));
			}
            //If we click a unit
//            else if (Input.GetMouseButtonDown(0) && !added && !selected && !Mouse.IsDragging)
//            {
//                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//                if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
//                {
//                    if (hitInfo.collider.gameObject.CompareTag("Selectable"))
//                    {
//                        mouse.unitsOnScreen.Add(hitInfo.collider.gameObject);
//                        selected = true;
//                        added = true;
//                        Bounds bigBounds = gameObject.GetComponentInChildren<Renderer>().bounds;
//
//                        // This "diameter" only works correctly for relatively circular or square objects
//                        float diameter = bigBounds.size.z;
//                        diameter *= 1.10f;
//
//                        selectionIndicator.SetActive(true);
//
//                        selectionIndicator.transform.position = new Vector3(bigBounds.center.x, 0.06f, bigBounds.center.z);
//                        selectionIndicator.transform.localScale = new Vector3(bigBounds.size.x + (diameter / 2.0f), bigBounds.size.y, bigBounds.size.z + (diameter / 2.0f));
//
//                    }
//                }
//            }
        }

        if(Mouse.ShiftKeyDown() && Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                if(hitInfo.collider.gameObject.GetInterface<ISelectable>() != null)
                {
                    Debug.Log("Removed one");
                    mouse.DeselectShiftedUnit(hitInfo.transform.gameObject);
                    UnitSelected unitHit = hitInfo.transform.gameObject.GetComponent<UnitSelected>();
                    unitHit.selected = false;
                    unitHit.added = false;
                    unitHit.selectionIndicator.SetActive(false);
                }
                else 
                {
                    Debug.Log("Removed all");
                    mouse.DeselectAllUnits(gameObject);
                    selected = false;
                    added = false;
                    selectionIndicator.SetActive(false);
                }
            }	
        }

		isFirst = mouse.IsFirstInList(gameObject);
    }
}
