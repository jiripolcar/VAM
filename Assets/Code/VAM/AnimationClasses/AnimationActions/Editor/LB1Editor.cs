using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LB1))]
public class LB1Editor : Editor
{
    private static float maxStepTime = 10;
    private float currentStepTime = 0;
    private static float viewStart = 0;
    private static float viewEnd = 1;
    private LB1 _lb;
    private LB1 lb { get { if (!_lb) _lb = (LB1)target; return _lb; } }
    private static bool previewMode = false;

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Fill list", GUILayout.Width(150))) lb.FillLineBenderElementsList();
        base.OnInspectorGUI();
        GUILayout.Space(10);        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("View elements:");
        if (GUILayout.Button("ALL",GUILayout.Width(50))) viewEnd = lb.lineBenderElements.Count;
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.MinMaxSlider(ref viewStart, ref viewEnd, 0, lb.lineBenderElements.Count);

        GUILayout.Space(10);
        maxStepTime = EditorGUILayout.FloatField("Step time: ", maxStepTime);
        GUILayout.Space(10);
        if (previewMode)
        {
            if (GUILayout.Button("Turn OFF preview", GUILayout.Width(150)))
                previewMode = false;
            GUILayout.Label("Preview time:");
            currentStepTime = EditorGUILayout.FloatField("", currentStepTime);
            currentStepTime = GUILayout.HorizontalSlider(currentStepTime, 0, maxStepTime);
            lb.SetTime(currentStepTime);
        }
        else
        {
            if (GUILayout.Button("Turn ON preview", GUILayout.Width(150)))
                previewMode = true;
        }
        GUILayout.Space(10);
        for (int i = Mathf.FloorToInt(viewStart); i < Mathf.CeilToInt(viewEnd) && i < lb.lineBenderElements.Count; i++)
            //foreach (LB1Element element in lb.lineBenderElements)
            if (lb.lineBenderElements[i] != null) DrawLB1Element(lb.lineBenderElements[i]);
        

    }

    private void DrawLB1Element(LB1Element element)
    {
        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Element: " + element.name);
        if (GUILayout.Button("Select", GUILayout.Width(50)))
            Selection.objects = new GameObject[] { element.gameObject };

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.MinMaxSlider(ref element.startTime, ref element.endTime, 0, maxStepTime);
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


        if (!element.IsScaleOne)
            GUILayout.Label("WARNING! Element parent's scale is not one! For scaling, scale the joints or bodies!");
       GUILayout.Space(10);
    }


}
