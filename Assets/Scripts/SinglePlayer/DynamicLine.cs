using System.Collections.Generic;
using UnityEngine;

public class DynamicLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public List<Transform> points;
    public Transform Rope;
    
    [ContextMenu("GetRopeComponents")]
    private void GetRopeComponents()
    {
        foreach (Transform segment in Rope)
        {
            points.Add(segment);
        }
    }
    
    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
        lineRenderer.positionCount = points.Count;
    }

    void Update()
    {
        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, points[i].position);
        }
    }
}