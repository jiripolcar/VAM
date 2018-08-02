using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LB1Element))]
public class LB1ElementEditor : Editor
{
    private LB1Element _lbe;
    private LB1Element lbe { get { if (!_lbe) _lbe = (LB1Element)target; return _lbe; } }




    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        lbe.Length = EditorGUILayout.DelayedFloatField("Length: ", lbe.Length);
        if (GUILayout.Button("Reposition Child Segments")) lbe.RepositionChildSegments();


        if (GUILayout.Button("Append new segment")) Selection.objects = new GameObject[] { lbe.AppendSegment() };

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Set FINAL"))
            {
                lbe.targetRotation = lbe.transform.localRotation;
            }
            if (GUILayout.Button("Set DEFAULT"))
            {
                lbe.defaultRotation = lbe.transform.localRotation;
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Parent look at, Set FINAL"))
        {
            lbe.transform.parent.gameObject.GetComponent<LB1Element>().ComputeTargetRotationAndLength(lbe.gameObject, true);
        }
        if (GUILayout.Button("Parent look at, Set DEFAULT"))
        {
            lbe.transform.parent.gameObject.GetComponent<LB1Element>().ComputeTargetRotationAndLength(lbe.gameObject, false);
        }
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Select previous"))
            Selection.objects = new GameObject[] { lbe.transform.parent.gameObject };

        EditorGUILayout.Space();
        GUILayout.TextArea("DO NOT FORGET TO SET MATERIAL BEFORE STARTING!");
        GUILayout.TextArea("Usage: Append new segment, position it. Then, click Set FINAL, which will set the parent's length and ending rotation.");

    }


}
