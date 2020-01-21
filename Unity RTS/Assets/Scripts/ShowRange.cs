using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class ShowRange : MonoBehaviour
{
    [Range(0, 256)]
    public int segments = 50;

    [HideInInspector]
    public float radius;

    LineRenderer lineRenderer;
    GhostBuilding ghostBuilding;
    RTSObject rtsObject;

    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        ghostBuilding = gameObject.GetComponent<GhostBuilding>();

        rtsObject = ghostBuilding.realBuilding.GetComponent<RTSObject>();

        radius = rtsObject.range;

        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = false;
        CreatePoints();
    }

    void CreatePoints()
    {
        float deltaTheta = (float)(2.0 * Mathf.PI) / segments;
        float theta = 0f;

        for (int i = 0; i < segments + 1; i++)
        {
            float x = radius * Mathf.Cos(theta);
            float z = radius * Mathf.Sin(theta);
            Vector3 pos = new Vector3(x, 0, z);
            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }
}