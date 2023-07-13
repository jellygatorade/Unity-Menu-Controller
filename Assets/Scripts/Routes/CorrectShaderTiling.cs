using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Material))]
[DisallowMultipleComponent]
public class CorrectShaderTiling : MonoBehaviour
{
    [SerializeField] private LineRenderer LR;
    [SerializeField] private CurvedLineRenderer CLR;
    private Material Mat;

    private void Reset()
    {   
        if (gameObject.GetComponent<LineRenderer>() != null)
        {
            LR = gameObject.GetComponent<LineRenderer>();
        }

        if (gameObject.GetComponent<CurvedLineRenderer>() != null)
        {
            CLR = gameObject.GetComponent<CurvedLineRenderer>();
        }

        if (LR != null && CLR != null)
        {   
            Mat = new Material(Shader.Find("Shader Graphs/DashingLineShader")); // Must be included in "Edit -> Project Settings -> Graphics -> Always Included Shaders"
            
            // Uniforms (Filling, Tiling, Color)

            Mat.SetFloat("_FillingValue", 1f); // 0.5f is half in negative direction, 1f is full, 1.5f is half in positive direction

            float length = GetTotalLength(LR);
            float width = CLR.lineWidth;
            // var BGCurveMath = Route.GetComponent<BGCcMath>();
            // var SplineLength = BGCurveMath.GetDistance();
            Mat.SetFloat("_Tiling", 0.25f * length / width);

            Color32 NCMAOrange = new Color32(224, 89, 42, 255);
            Mat.SetColor("_Color", NCMAOrange);

            LR.sharedMaterial = Mat;
        }
    }

    private void OnValidate() 
    {
        // Debug.Log("On validate shader tiling");
    }

    private float GetTotalLength(LineRenderer LineRenderer)
    {
        Vector3[] pointsInLine;
        pointsInLine = new Vector3[LineRenderer.positionCount]; // array vector3 must be created first with LineRenderer.positionCount before using LineRenderer.GetPositions

        LineRenderer.GetPositions(pointsInLine);

        float sum = 0f;

        for (int i = 1; i < pointsInLine.Length; i++)
        {
            float dist = Vector3.Distance(pointsInLine[i - 1], pointsInLine[i]);
            sum += dist;
        }

        return sum;
    }
}
