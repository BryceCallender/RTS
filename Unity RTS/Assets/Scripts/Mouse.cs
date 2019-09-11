using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour 
{
    RaycastHit hitInfo;

    //Stores all the gameobjects in the box 
    public List<GameObject> selectedObjects = new List<GameObject>();
    public List<GameObject> unitsOnScreen = new List<GameObject>();
    public static Vector3 mouseClick;

    public GUIStyle mouseDragSkin;

    private Vector3 mouseDownPosition;
    private Vector3 currentMousePosition;
	private Vector2 boxStart;
	private Vector2 boxFinish;


    private float timeToMakeDragBox = 1f;
    private float timeLeftBeforeDragBox;

    public static bool IsDragging = false;
    public static bool IsClickedAway = false;
	public bool isFirst = false;

    private void Awake()
    {
        currentMousePosition = Vector3.zero;
        mouseDownPosition = Vector3.zero;
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray,out hitInfo,Mathf.Infinity))
        {
            currentMousePosition = hitInfo.point;
            if(Input.GetMouseButtonDown(0))
            {
                DeselectAllUnits();
                mouseDownPosition = hitInfo.point;
                timeLeftBeforeDragBox = timeToMakeDragBox;
                if(!IsDragging)
                {
                    timeLeftBeforeDragBox -= Time.deltaTime;
                    if (timeLeftBeforeDragBox <= 0)
                    {
                        IsDragging = true;
                    }

                }
            }
            else if(Input.GetMouseButtonUp(0))
            {
                timeLeftBeforeDragBox = 0;
                IsDragging = false;
                currentMousePosition = hitInfo.point;
            }
        }
    }

    private void OnGUI()
	{
        float boxWidth = Camera.main.WorldToScreenPoint(mouseDownPosition).x - Camera.main.WorldToScreenPoint(currentMousePosition).x;
        float boxHeight = Camera.main.WorldToScreenPoint(mouseDownPosition).y - Camera.main.WorldToScreenPoint(currentMousePosition).y;
        float boxLeft, boxTop;

        boxTop = (Screen.height - Input.mousePosition.y) - boxHeight;
        boxLeft = Input.mousePosition.x;

        if (boxWidth > 0f && boxHeight > 0f)
        {
            boxStart = new Vector2(Input.mousePosition.x,Input.mousePosition.y + boxHeight);
        }
        else if (boxWidth < 0f && boxHeight > 0f)
        {
            boxStart = new Vector2(Input.mousePosition.x + boxWidth, Input.mousePosition.y + boxHeight);
        }
        else if (boxWidth > 0f && boxHeight < 0f)
        {
            boxStart = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        else if(boxWidth < 0f && boxHeight < 0f)
        {
            boxStart = new Vector2(Input.mousePosition.x + boxWidth, Input.mousePosition.y);
        }

        boxFinish = new Vector2(boxStart.x + Mathf.Abs(boxWidth),boxStart.y - Mathf.Abs(boxHeight));
	    
        if (Event.current.type == EventType.MouseDrag)
		{
            if (!IsDragging)
			{
                IsDragging = true;
			}
		}

		if (Event.current.type == EventType.MouseUp)
		{
            IsDragging = false;
		}

        if (IsDragging)
        {
            GUI.Box(new Rect(boxLeft,boxTop,boxWidth,boxHeight),"",mouseDragSkin);
        }
	}

    private void LateUpdate()
    {
        if(IsDragging && unitsOnScreen.Count > 0)
        {
            for (int i = 0; i < unitsOnScreen.Count; i++)
            {
                GameObject unit = unitsOnScreen[i] as GameObject;
                UnitSelected unitSelectedScript = unit.GetComponent<UnitSelected>();
                if(UnitInDragBox(unitSelectedScript.ScreenPos)
                   && unitSelectedScript.selected 
                   && !UnitInListAlready(unit))
                {
                    selectedObjects.Add(unit);
                }
            }
        }
    }

    public bool UnitInDragBox(Vector2 screenPos)
    {
        if((screenPos.x > boxStart.x && screenPos.y < boxStart.y) &&
           (screenPos.x < boxFinish.x && screenPos.y > boxFinish.y))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public bool UnitInsideScreen(Vector2 screenPos)
    {
        if((screenPos.y < Screen.height && screenPos.x < Screen.width) 
           && (screenPos.x > 0 && screenPos.y > 0))
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    public void DeselectAllUnits(GameObject obj)
    {
		for (int i = 0; i < selectedObjects.Count; i++)
		{
		    GameObject unit = selectedObjects[i] as GameObject;
            UnitSelected unitSelectedScript = unit.GetComponent<UnitSelected>();
		    if (obj == unit)
		    {
	              unitSelectedScript.selected = false;
	              selectedObjects.Remove(obj);
		    }
		}
        unitsOnScreen.Clear();
	}

    public void DeselectAllUnits()
    {
        for (int i = 0; i < selectedObjects.Count; i++)
        {
            selectedObjects.Remove(selectedObjects[i]);
        }
    }

    public bool UnitInListAlready(GameObject obj)
    {
        for (int i = 0; i < selectedObjects.Count; i++)
        {
            GameObject unit = selectedObjects[i] as GameObject;
            if(obj == unit)
            {
                return true;
            }
        }
        return false;
    }

    public static bool ShiftKeyDown()
    {
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DeselectShiftedUnit(GameObject obj)
    {
        selectedObjects.Remove(obj);
        unitsOnScreen.Clear();
    }

	public bool IsFirstInList(GameObject unitName)
	{    
		if (selectedObjects.Count == 0 || unitName == null)
		{
			isFirst = false;
			return isFirst;
		}

		if(unitName.name == selectedObjects[0].name)
		{
			isFirst = true;
		}
		else
		{
			isFirst = false;
		}

		return isFirst;
	}
}
