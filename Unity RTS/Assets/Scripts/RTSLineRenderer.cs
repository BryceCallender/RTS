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

    private int pointCount;

    private void Start()
    {
        pointCount = 0;
        pointPositions = new List<Vector3>();
        lineRenderer = GetLineRendererFromLineMode();
    }

    public void SetPointCount(int pointCount)
    {
        this.pointCount = pointCount;
        lineRenderer.positionCount = pointCount;
    }

    public void AddLinePoint(Vector3 newPointPosition)
    {
        pointCount++;
        lineRenderer.positionCount = pointCount;

        pointPositions.Add(newPointPosition);
        lineRenderer.SetPosition(pointCount - 1, newPointPosition);
    }

    public void ChangeLineMode(LineMode newLineMode)
    {
        lineMode = newLineMode;
        lineRenderer = GetLineRendererFromLineMode();
    }

    public void UpdatePointPosition(int index, Vector3 updatePosition)
    {
        lineRenderer.SetPosition(index, updatePosition);
        pointPositions[index] = updatePosition;
    }

    public void RemovePoint(int index)
    {
        //If i want to delete the 0th point im really thinking of the first point i have made,
        //however that is not the point that is really at that index, 0 is reserved for the home position of the object
        pointPositions.RemoveAt(index + 1);

        //Redraw based on the point position removed
        lineRenderer.positionCount = pointPositions.Count;
        pointCount = pointPositions.Count;
        for(int i = 0; i < pointPositions.Count; i++)
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

}
