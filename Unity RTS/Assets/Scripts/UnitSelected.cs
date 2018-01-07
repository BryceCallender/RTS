using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelected : MonoBehaviour 
{
    public Vector2 ScreenPos;
    public int health;
    public bool selected;
    public bool added;
	public bool isFirst;
    public Mouse mouse;

    private new Renderer renderer;
    private GameObject selectionIndicator;
    private RaycastHit hitInfo;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        selectionIndicator = transform.Find("SelectionIndicator").gameObject;
        mouse = FindObjectOfType<Mouse>();
    }

    private void Update()
    {
        ScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        if (mouse.UnitInsideScreen(ScreenPos))
        {
            if (mouse.UnitInDragBox(ScreenPos) && !added && !selected && Mouse.IsDragging)
            {
                mouse.unitsOnScreen.Add(this.gameObject);
                selected = true;
                added = true;
				Bounds bigBounds = this.gameObject.GetComponentInChildren<Renderer>().bounds;

				// This "diameter" only works correctly for relatively circular or square objects
				float diameter = bigBounds.size.z;
				diameter *= 1.10f;

                selectionIndicator.SetActive(true);

                selectionIndicator.transform.position = new Vector3(bigBounds.center.x, 0.06f, bigBounds.center.z);
                selectionIndicator.transform.localScale = new Vector3(bigBounds.size.x + (diameter / 2.0f), bigBounds.size.y, bigBounds.size.z + (diameter / 2.0f));

			}

        }

        if(Mouse.ShiftKeyDown() && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                if (hitInfo.collider.gameObject.CompareTag("Selectable"))
                {
                    Debug.Log("Removed one");
                    mouse.DeselectShiftedUnit(hitInfo.transform.gameObject);
                    //TODO::fix these 3 getcomponents to make it easier on the complier
                    hitInfo.transform.gameObject.GetComponent<UnitSelected>().selected = false;
                    hitInfo.transform.gameObject.GetComponent<UnitSelected>().added = false;
                    hitInfo.transform.gameObject.GetComponent<UnitSelected>().selectionIndicator.SetActive(false);
                }
                else 
                {
                    Debug.Log("Removed all");
                    mouse.DeselectAllUnits(this.gameObject);
                    selected = false;
                    added = false;
                    selectionIndicator.SetActive(false);
                }
            }	
        }

		isFirst = mouse.IsFirstInList(this.gameObject);
    }
}
