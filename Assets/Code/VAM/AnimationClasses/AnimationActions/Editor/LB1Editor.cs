using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LB1))]
public class LB1Editor : Editor
{
    private float stepTime = 10;
    private float currentStepTime = 0;
    private LB1 _lb;
    private LB1 lb { get { if (!_lb) _lb = (LB1)target; return _lb; } }
    private bool previewMode = false;

    public override void OnInspectorGUI()
    {        
        base.OnInspectorGUI();
        GUILayout.Label("Editor mode begin:");
        stepTime = EditorGUILayout.FloatField("Step time: ", stepTime);
        if (previewMode)
        {
            if (GUILayout.Button("Turn OFF preview"))
                previewMode = false;
            GUILayout.Label("Preview time:");
            currentStepTime = GUILayout.HorizontalSlider(currentStepTime, 0, stepTime);
            lb.SetTime(currentStepTime);
        }
        else
        {
            if (GUILayout.Button("Turn ON preview"))
                previewMode = true;
        }
           foreach (LB1Element element in lb.lineBenderElements)
            if (element != null) DrawLB1Element(element);
        if (GUILayout.Button("Fill list")) lb.FillLineBenderElementsList();
        
    }

    private void DrawLB1Element(LB1Element element)
    {
        GUILayout.Label("Element: " + element.name);
        EditorGUILayout.MinMaxSlider(ref element.startTime, ref element.endTime, 0, stepTime);
        EditorGUILayout.BeginHorizontal();
        element.startTime = EditorGUILayout.FloatField(element.startTime);
        element.endTime = EditorGUILayout.FloatField(element.endTime);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        element.bendingAngle = EditorGUILayout.FloatField("Bend: ", element.bendingAngle);
        element.Length = EditorGUILayout.FloatField("Length: ", element.Length);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Total yaw:" + element.GetTotalYawRecursive().ToString("0.000"));
        element.yawAngle = EditorGUILayout.FloatField("Yaw: ", element.yawAngle);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Select"))
            Selection.objects = new GameObject[] { element.gameObject };

        if (!element.IsScaleOne)
            GUILayout.Label("WARNING! Element parent's scale is not one! For scaling, scale the joints or bodies!");
        GUILayout.Label("");

    }

    
}
