using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeManager : MonoBehaviour
{
    public GameObject segmentPrefab;
    public int segmentCount = 10;
    public float segmentDistance = 0.1f;
    public float springForce = 50f;
    public float damperForce = 5f;
    public GameObject ropeParent;

    
    [ContextMenu("RopeManager")]
    void CreateSpringRope()
    {
        GameObject previousSegment = null;

        for (int i = 0; i < segmentCount; i++)
        {
            GameObject newSegment = Instantiate(segmentPrefab, transform);
            newSegment.transform.localPosition = new Vector3(0, -segmentDistance * i, 0);

            Rigidbody rb = newSegment.AddComponent<Rigidbody>();
            SpringJoint sj = newSegment.AddComponent<SpringJoint>();

            sj.connectedBody = previousSegment != null ? previousSegment.GetComponent<Rigidbody>() : null; // First segment is not connected to anything
            sj.spring = springForce;
            sj.damper = damperForce;
            sj.minDistance = segmentDistance * 0.8f;
            sj.maxDistance = segmentDistance;
            previousSegment = newSegment;
        }
    }
}
