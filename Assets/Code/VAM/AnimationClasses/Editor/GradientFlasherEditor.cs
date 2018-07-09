using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GradientFlasher))]
public class GradientFlasherEditor : Editor
{
    public override void OnInspectorGUI()
    {
            base.OnInspectorGUI();
        if (GUILayout.Button("Fill materials"))
        {
            ((GradientFlasher)target).FillMaterials();
        }
    }

}
