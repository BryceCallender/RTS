using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LineMode
{
   Attack,
   Move,
   Patrol
}

public class RTSLineRenderer : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private LineMode lineMode = LineMode.Move;

    [SerializeField]
    private List<Vector3> pointPositions;
    [SerializeField]
    private List<GameObject> locationMarkers;

    private int pointCount
    {
        get
        {
            return lineRenderer.positionCount;
        }
    }

    private void Start()
    {
        pointPositions = new List<Vector3>();
        locationMarkers = new List<GameObject>();
        lineRenderer = GetLineRendererFromLineMode();
        AddLinePoint(transform.position); //Home point
    }

    //Use for updating the very first point in the renderer constantly :)
    private void Update()
    {
        if(lineRenderer.gameObject.activeSelf && pointCount > 0)
        {
            UpdatePointPosition(0, transform.position);
        }
    }

    public void SetPointCount(int pointCount)
    {
        lineRenderer.positionCount = pointCount;
    }

    public void AddLinePointToFront(Vector3 position)
    {
        lineRenderer.positionCount++;

        pointPositions.Insert(1, position);

        for(int i = 0; i < pointCount; i++)
        {
            lineRenderer.SetPosition(i, pointPositions[i]);
        }
    }

    public void AddLinePoint(Vector3 newPointPosition)
    {
        lineRenderer.positionCount++;

        pointPositions.Add(newPointPosition);
        lineRenderer.SetPosition(pointCount - 1, newPointPosition);

        if(pointCount > 1)
        {
            locationMarkers.Add(CreateLocationMarker(newPointPosition));
        }

    }

    public void ChangeLineMode(LineMode newLineMode)
    {
        lineMode = newLineMode;
        lineRenderer = GetLineRendererFromLineMode();
        AddLinePoint(transform.position); //Home point
    }

    public void UpdatePointPosition(int index, Vector3 updatePosition)
    {
        lineRenderer.SetPosition(index, updatePosition);
        pointPositions[index] = updatePosition;

        //If we even have a marker to update that can corelate to the updating index
        //The origin is itself which should not have a marker
        if(index > 0)
        {
            locationMarkers[index - 1].transform.position = updatePosition;
        }
    }

    public void RemovePoint(int index)
    {
        pointPositions.RemoveAt(index);

        Destroy(locationMarkers[index]);
        locationMarkers.RemoveAt(index);

        //Redraw based on the point position removed
        lineRenderer.positionCount = pointPositions.Count;

        for(int i = 0; i < pointCount; i++)
        {
            lineRenderer.SetPosition(i, pointPositions[i]);
        }
    }

    public int GetPointCount()
    {
        return pointCount;
    }

    private LineRenderer GetLineRendererFromLineMode()
    {
        //Kill references if its to make anew
        if (lineRenderer != null)
            Destroy(lineRenderer.gameObject);

        switch (lineMode)
        {
            case LineMode.Attack: 
                return Mouse.InstantiateRTSEffect("attackLine", Vector3.zero, transform).GetComponent<LineRenderer>();
            case LineMode.Move: 
                return Mouse.InstantiateRTSEffect("movementLine", Vector3.zero, transform).GetComponent<LineRenderer>();
            case LineMode.Patrol: 
                return Mouse.InstantiateRTSEffect("patrolLine", Vector3.zero, transform).GetComponent<LineRenderer>();
            default: 
                return null;
        }
    }

    private GameObject CreateLocationMarker(Vector3 location)
    {
        switch (lineMode)
        {
            case LineMode.Attack:
                return Mouse.InstantiateRTSEffect("attackWaypoint", location);
            case LineMode.Move:
                return Mouse.InstantiateRTSEffect("movementWaypoint", location);
            case LineMode.Patrol:
                return Mouse.InstantiateRTSEffect("patrolWaypoint", location);
            default:
                return null;
        }
    }

    public void Show()
    {
        //If already shown no work is to be done :)
        if (lineRenderer.gameObject.activeSelf)
            return; 

        lineRenderer.gameObject.SetActive(true);

        foreach(GameObject marker in locationMarkers)
        {
            marker.SetActive(true);
        }
    }

    public void Hide()
    {
        //If already hidden no work is to be done :)
        if (!lineRenderer.gameObject.activeSelf)
            return;

        lineRenderer.gameObject.SetActive(false);

        foreach (GameObject marker in locationMarkers)
        {
            marker.SetActive(false);
        }
    }

    public void ClearLineRenderer()
    {
        pointPositions.Clear();
        lineRenderer.positionCount = 0;

        foreach(GameObject marker in locationMarkers)
        {
            Destroy(marker);
        }

        locationMarkers.Clear();

        AddLinePoint(transform.position); //Home point readded
    }

}
