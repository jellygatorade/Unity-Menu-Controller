using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Mathematics;
using UnityEngine.Splines;
using UnityEditor;

[ExecuteInEditMode()]
public class SplineSampler : MonoBehaviour
{
    [SerializeField]
    private SplineContainer m_splineContainer;
    
    [SerializeField]
    private int m_splineIndex;

    [SerializeField]
    [Range(0f, 1f)]
    private float m_time;

    [SerializeField]
    private float m_width;

    // public float SampleSplineWidth { get { return m_width; } }

    float3 position;
    float3 tangent;
    float3 upVector;

    float3 p1;
    float3 p2;

    private void Update()
    {
        m_splineContainer.Evaluate(m_splineIndex, m_time, out position, out tangent, out upVector);

        // Tangent is the (forward) direction of travel along the spline to the next point;
        // Find the *right* direction based on this.
        float3 forward = tangent;
        float3 right = Vector3.Cross(forward, upVector).normalized;
        p1 = position + (right * m_width);
        p2 = position + (-right * m_width);
    }

    private void OnDrawGizmos()
    {
        //Handles.matrix = transform.localToWorldMatrix;
        Handles.SphereHandleCap(0, position, Quaternion.identity, 1f, EventType.Repaint);
        Handles.SphereHandleCap(1, p1, Quaternion.identity, 1f, EventType.Repaint);
        Handles.SphereHandleCap(2, p2, Quaternion.identity, 1f, EventType.Repaint);
    }

    // Working here
    // Get two points +-m_width from the spline at a certain distance t along its axis
    // t -  a value between 0 and 1 that represents the distance ratio along the curve
    public void SampleSplineWidth(float t, out Vector3 point1, out Vector3 point2)
    {
        m_splineContainer.Evaluate(m_splineIndex, t, out position, out tangent, out upVector);

        // Tangent is the (forward) direction of travel along the spline to the next point;
        // Find the *right* direction based on this.
        float3 forward = tangent;
        float3 right = Vector3.Cross(forward, upVector).normalized;
        point1 = position + (right * m_width);
        point2 = position + (-right * m_width);
    }
}
