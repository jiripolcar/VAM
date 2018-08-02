using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LB1))]
public class LB1Editor : Editor
{
    private static float indendedDuration = 10;
    private static float currentTime = 0;
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
        int intViewStart = Mathf.FloorToInt(viewStart);
        int intViewEnd = Mathf.CeilToInt(viewEnd);

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
            newMat = (Material)EditorGUILayout.ObjectField(newMat, typeof(Material), true);
            if (GUILayout.Button("Set material") && newMat != null) lb.MultipleSetMaterial(intViewStart, intViewEnd, newMat);
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
                lb.MultipleSet(intViewStart, intViewEnd, viewMultipleStartTime, viewMultipleEndTime);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Save as DEFAULT"))
                SaveAsDefault(lb.lineBenderElements.GetRange(intViewStart, intViewEnd - intViewStart));
            if (GUILayout.Button("Swap DEFAULT FINAL"))
                SwapDefaultFinal(lb.lineBenderElements.GetRange(intViewStart, intViewEnd - intViewStart));
            if (GUILayout.Button("Save as FINAL"))
                SaveAsFinal(lb.lineBenderElements.GetRange(intViewStart, intViewEnd - intViewStart));
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Zero rotation"))
            ZeroRotations(lb.lineBenderElements.GetRange(intViewStart, intViewEnd - intViewStart));


        GUILayout.Space(10);
        indendedDuration = EditorGUILayout.FloatField("Intended duration: ", indendedDuration);
        GUILayout.Space(10);
        if (previewMode)
        {
            if (GUILayout.Button("Turn OFF preview"))
                previewMode = false;
            GUILayout.Label("Preview time:");
            currentTime = EditorGUILayout.FloatField("", currentTime);
            currentTime = GUILayout.HorizontalSlider(currentTime, 0, indendedDuration);
            lb.SetLerp(currentTime / indendedDuration);
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
        for (int i = intViewStart; i < intViewEnd && i < lb.lineBenderElements.Count; i++)
            if (lb.lineBenderElements[i] != null) DrawLB1Element(lb.lineBenderElements[i], i);
    }

    private void OnSceneGUI()
    {
        int intViewStart = Mathf.FloorToInt(viewStart);
        int intViewEnd = Mathf.CeilToInt(viewEnd);
        for (int i = intViewStart; i < intViewEnd && i < lb.lineBenderElements.Count; i++)
            if (lb.lineBenderElements[i] != null)
                Handles.Label(lb.lineBenderElements[i].transform.position, i.ToString());
    }

    private void SwapDefaultFinal(List<LB1Element> elements)
    {
        elements.ForEach((lb1e) => Helpers.Swap<Quaternion>(ref lb1e.defaultRotation, ref lb1e.targetRotation));

        /*{
            Quaternion buffer = lb1e.defaultRotation;
            lb1e.defaultRotation = lb1e.targetRotation;
            lb1e.targetRotation = buffer;
        });*/

    }

    private void SaveAsDefault(List<LB1Element> elements)
    {
        elements.ForEach((lb1e) => lb1e.SaveDefaultRotation());
    }

    private void ZeroRotations(List<LB1Element> elements)
    {
        elements.ForEach((lb1e) => lb1e.transform.localRotation = Quaternion.identity);
    }

    private void SaveAsFinal(List<LB1Element> elements)
    {
        elements.ForEach((lb1e) => lb1e.SaveTargetRotation());
    }




    private void DrawLB1Element(LB1Element element, int? index = null)
    {
        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Select", GUILayout.Width(50)))
                Selection.objects = new GameObject[] { element.gameObject };
            GUILayout.Label(index == null ? "" : index.ToString() + "    " + element.name, GUILayout.MaxWidth(300));
        }
        float startTime = element.startLerp * indendedDuration;
        float endTime = element.endLerp * indendedDuration;
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        {
            startTime = EditorGUILayout.FloatField(startTime, GUILayout.Width(50));
            EditorGUILayout.MinMaxSlider(ref startTime, ref endTime, 0, indendedDuration);
            endTime = EditorGUILayout.FloatField(endTime, GUILayout.Width(50));
        }

        element.startLerp = startTime / indendedDuration;
        element.endLerp = endTime / indendedDuration;

        EditorGUILayout.EndHorizontal();


        //    element.Length = EditorGUILayout.FloatField("Length: ", element.Length);

        if (!element.IsScaleOne)
            GUILayout.Label("WARNING! Element parent's scale is not one! For scaling, scale the joints or bodies!");
        GUILayout.Space(10);


    }


}
