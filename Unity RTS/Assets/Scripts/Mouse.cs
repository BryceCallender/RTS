using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct VisualEffect
{
    public string name;
    public GameObject effect;
}

public class Mouse : MonoBehaviour 
{
    public Texture2D[] cursorTextures;
    public VisualEffect[] rtsVisualEffectList;

    RaycastHit hitInfo;

    //Stores all the gameobjects in the box 
    public List<GameObject> selectedObjects = new List<GameObject>();
    public List<GameObject> unitsOnScreen = new List<GameObject>();
    public static Vector3 mouseClick;

    public static Dictionary<string, Texture2D> rtsCursorEffects = new Dictionary<string, Texture2D>();
    public static Dictionary<string, GameObject> rtsVisualEffects = new Dictionary<string, GameObject>();

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

    private Camera camera;

    private readonly static Vector3 offset = new Vector3(0, 0.05f, 0);

    private void Awake()
    {
        currentMousePosition = Vector3.zero;
        mouseDownPosition = Vector3.zero;
        camera = Camera.main;

        Cursor.SetCursor(cursorTextures[0], Vector2.zero, CursorMode.Auto);

        foreach(VisualEffect visualEffect in rtsVisualEffectList)
        {
            rtsVisualEffects.Add(visualEffect.name, visualEffect.effect);
        }

        foreach(Texture2D texture in cursorTextures)
        {
            rtsCursorEffects.Add(texture.name.Substring(texture.name.IndexOf('_') + 1), texture);
        }
    }

    private void Update()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray,out hitInfo,Mathf.Infinity))
        {
            currentMousePosition = hitInfo.point;
            if(Input.GetMouseButtonDown(0))
            {
                ChangeCursor("normal");
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

            //Handling Unit clicking effects
            if(Input.GetMouseButtonDown(1))
            {
                //If we want to move the objects
                if(selectedObjects.Count > 0)
                {
                    //We clicked on an enemy so use the enemy attack indicator
                    if(hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                    {
                        InstantiateRTSEffect("attackIndicator", hitInfo.point);
                        //InstantiateRTSEffect("targetLocked", hitInfo.point);
                    }
                    else //Just telling the unit to move elsewhere
                    {
                        InstantiateRTSEffect("moveIndicator", hitInfo.point);
                    }
                    
                }
            }
        }
    }

    private void OnGUI()
    {
        float boxWidth = camera.WorldToScreenPoint(mouseDownPosition).x - camera.WorldToScreenPoint(currentMousePosition).x;
        float boxHeight = camera.WorldToScreenPoint(mouseDownPosition).y - camera.WorldToScreenPoint(currentMousePosition).y;
        float boxLeft, boxTop;

        boxTop = (Screen.height - Input.mousePosition.y) - boxHeight;
        boxLeft = Input.mousePosition.x;

        if (boxWidth > 0f && boxHeight > 0f)
        {
            boxStart = new Vector2(Input.mousePosition.x, Input.mousePosition.y + boxHeight);
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

        boxFinish = new Vector2(boxStart.x + Mathf.Abs(boxWidth), boxStart.y - Mathf.Abs(boxHeight));
	    
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
            GUI.Box(new Rect(boxLeft, boxTop, boxWidth, boxHeight), "", mouseDragSkin);
        }
    }

    private void LateUpdate()
    {
        if(IsDragging && unitsOnScreen.Count > 0)
        {
            foreach(GameObject unit in unitsOnScreen)
            {
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

    public void DeselectAllUnits()
    {
        foreach(GameObject unit in selectedObjects)
        {
            UnitSelected unitSelectedScript = unit.GetComponent<UnitSelected>();

            unitSelectedScript.selected = false;
            unitSelectedScript.added = false;
            unitSelectedScript.selectionIndicator.SetActive(false);
        }

        selectedObjects.Clear();
        unitsOnScreen.Clear();
    }

    public bool UnitInListAlready(GameObject obj)
    {
        return selectedObjects.Contains(obj);
    }

    public static bool ShiftKeyDown()
    {
        return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
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

    public static GameObject InstantiateRTSEffect(string effectName, Vector3 position, Transform parent = null)
    {
        return Instantiate(rtsVisualEffects[effectName], position.Flatten() + offset, Quaternion.identity, parent);
    }

    public void ChangeCursor(string cursorName)
    {
        Cursor.SetCursor(rtsCursorEffects[cursorName], Vector2.zero, CursorMode.Auto);
    }
}
