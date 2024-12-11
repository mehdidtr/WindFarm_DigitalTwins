using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProcessData))]
public class ProcessDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ProcessData processData = (ProcessData)target;
        if (GUILayout.Button("Generate Turbine Data"))
        {
            processData.GenerateTurbineData();
        }
    }
}