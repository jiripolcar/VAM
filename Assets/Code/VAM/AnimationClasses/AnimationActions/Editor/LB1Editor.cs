using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LB1))]
public class LB1Editor : Editor
{
    private float stepTime = 100;
    private LB1 _lb;
    private LB1 lb { get { if (!_lb) _lb = (LB1)target; return _lb; } }

    public override void OnInspectorGUI()
    {
        stepTime = EditorGUILayout.FloatField("Step time: ", stepTime);
        base.OnInspectorGUI();
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
        element.bendingAngle = EditorGUILayout.FloatField("a: ", element.bendingAngle);
    }

    
}
