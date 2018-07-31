using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LB1))]
public class LB1Editor : Editor
{
    private static float maxStepTime = 10;
    private static float currentStepTime = 0;
    private static float viewStart = 0;
    private static float viewEnd = 1;
    private LB1 _lb;
    private LB1 lb { get { if (!_lb) _lb = (LB1)target; return _lb; } }
    private static bool previewMode = false;
    private static float viewMultipleStartTime = 1;
    private static float viewMultipleEndTime = 3;
    private Material newMat;

    public override void OnInspectorGUI()
    {
        GUILayout.TextArea("Do not forget to fill list before trying.");
        if (GUILayout.Button("Fill list")) lb.FillLineBenderElementsList();
        base.OnInspectorGUI();
        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Label("View elements:");
            if (GUILayout.Button("ALL", GUILayout.Width(50))) viewEnd = lb.lineBenderElements.Count;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            newMat =  (Material)EditorGUILayout.ObjectField(newMat, typeof(Material), true);
            if (GUILayout.Button("Set material") && newMat != null) lb.MultipleSetMaterial(Mathf.FloorToInt(viewStart), Mathf.CeilToInt(viewEnd), newMat);
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.MinMaxSlider(ref viewStart, ref viewEnd, 0, lb.lineBenderElements.Count);

        GUILayout.Label("Set multiple start and end times:");
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginHorizontal();
            {
                viewMultipleStartTime = EditorGUILayout.FloatField("", viewMultipleStartTime, GUILayout.Width(50));
                viewMultipleEndTime = EditorGUILayout.FloatField("", viewMultipleEndTime, GUILayout.Width(50));
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Set", GUILayout.Width(50)))
                lb.MultipleSet(Mathf.FloorToInt(viewStart), Mathf.CeilToInt(viewEnd), viewMultipleStartTime, viewMultipleEndTime);
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
        maxStepTime = EditorGUILayout.FloatField("Step time: ", maxStepTime);
        GUILayout.Space(10);
        if (previewMode)
        {
            if (GUILayout.Button("Turn OFF preview"))
                previewMode = false;
            GUILayout.Label("Preview time:");
            currentStepTime = EditorGUILayout.FloatField("", currentStepTime);
            currentStepTime = GUILayout.HorizontalSlider(currentStepTime, 0, maxStepTime);
            lb.SetLerp(currentStepTime);
        }
        else
        {
            if (GUILayout.Button("Turn ON preview"))
                previewMode = true;
            GUILayout.TextArea("Once done, you can make a dead copy of this (in any step time).");
            if (GUILayout.Button("Copy & Collapse"))
                lb.Collapse();
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
        {
            if (GUILayout.Button("Select", GUILayout.Width(50)))
                Selection.objects = new GameObject[] { element.gameObject };
            GUILayout.Label(element.name, GUILayout.MaxWidth(300));
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.MinMaxSlider(ref element.startTime, ref element.endTime, 0, maxStepTime);
        EditorGUILayout.BeginHorizontal();
        {
            element.startTime = EditorGUILayout.FloatField(element.startTime);
            element.endTime = EditorGUILayout.FloatField(element.endTime);
        }
        EditorGUILayout.EndHorizontal();


        //    element.Length = EditorGUILayout.FloatField("Length: ", element.Length);

        if (!element.IsScaleOne)
            GUILayout.Label("WARNING! Element parent's scale is not one! For scaling, scale the joints or bodies!");
        GUILayout.Space(10);


    }


}
