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


    }
}
