using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[ExecuteInEditMode()]
public class SplinePlane : MonoBehaviour
{
    [SerializeField]
    SplineSampler m_splineSampler;

    [SerializeField]
    int resolution;

    private List<Vector3> m_vertsP1;
    private List<Vector3> m_vertsP2;

    private void Update()
    {
        GetVerts();
    }

    private void GetVerts()
    {
        m_vertsP1 = new List<Vector3>();
        m_vertsP2 = new List<Vector3>();

        float step = 1f / (float)resolution;
        for (int i = 0; i < resolution + 1; i++) // + 1 to cap end point
        {
            float t = step * i;
            m_splineSampler.SampleSplineWidth(t, out Vector3 p1, out Vector3 p2);
            m_vertsP1.Add(p1);
            m_vertsP2.Add(p2);
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < m_vertsP1.Count; i++)
        {
            Handles.SphereHandleCap(1, m_vertsP1[i], Quaternion.identity, 1f, EventType.Repaint);
        }
        for (int i = 0; i < m_vertsP2.Count; i++)
        {
            Handles.SphereHandleCap(1, m_vertsP2[i], Quaternion.identity, 1f, EventType.Repaint);
        }
    }
}
