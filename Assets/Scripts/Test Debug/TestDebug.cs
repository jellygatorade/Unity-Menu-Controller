using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TestDebugScriptableObject", order = 1)]
public class TestDebug : ScriptableObject
{
    public void Log1()
    {
        Debug.Log("Test debug 1");
    }

    public void Log2()
    {
        Debug.Log("Test debug 2");
    }
}
