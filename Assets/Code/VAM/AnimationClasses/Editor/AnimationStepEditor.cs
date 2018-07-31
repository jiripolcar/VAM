using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Animations
{

    [CustomEditor(typeof(AnimationStep))]
    public class AnimationStepEditor : Editor
    {
        private bool previewMode = false;
        private bool stepTimingMode = false;
        private float currentStepTime = 0;
        private static bool showDefaultInspector;

        public override void OnInspectorGUI()
        {
            AnimationStep step = (AnimationStep)target;

            GUILayout.TextArea(
                "Place static step objects under the AR and Non-AR corresponding Animation Object Parent GameObjects.\n" +
                "If you intend using AR, the Non-AR objects will be hidden when tracking is found. If you don't intend to, place everything under the AR GameObject.\n"
                );

            if (GUILayout.Button("Setup Static Objects")) step.SetupStaticObjects();

            GUILayout.TextArea("Set up the Camera position.");
            if (GUILayout.Button("SAVE Camera Position")) SaveCameraPosition();
            if (GUILayout.Button("LOAD Camera Position")) LoadCameraPosition();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Step length:");
            step.stepLength = EditorGUILayout.DelayedFloatField(step.stepLength);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("End style");
            step.stepEnd = (StepEndAction)EditorGUILayout.EnumPopup((System.Enum)step.stepEnd);
            EditorGUILayout.EndHorizontal();

            GUILayout.TextArea("After you've aligned the static objects, create animation elements.");
            if (GUILayout.Button("Create AnimationElement")) NewElement();

            GUILayout.TextArea("Preview mode: You can load static objects and preview the animation.");
            if (GUILayout.Button("Reload static objects")) ResetObjects();
            if (GUILayout.Button("Unload static objects")) Unload();

            GUILayout.Space(16);

            if (previewMode && !stepTimingMode)
            {
                if (GUILayout.Button("Turn OFF preview"))
                    previewMode = false;

                GUILayout.Label("Preview time:");
                currentStepTime = EditorGUILayout.FloatField("", currentStepTime);
                currentStepTime = GUILayout.HorizontalSlider(currentStepTime, 0, step.stepLength);
                step.Animate(currentStepTime);
            }
            else
            {
                if (GUILayout.Button("Turn ON preview"))
                {
                    previewMode = true;
                    stepTimingMode = false;
                }
            }


            if (stepTimingMode && !previewMode)
            {
                if (GUILayout.Button("Turn OFF Step Timing Mode"))
                    stepTimingMode = false;

                step.animationElements.ForEach((ae) => DrawAETimes(ae, step));
            }
            else
            {
                if (GUILayout.Button("Turn ON Step Timing Mode"))
                {
                    previewMode = false;
                    stepTimingMode = true;
                }
            }

            GUILayout.Space(16);


            if (showDefaultInspector)
            {
                if (GUILayout.Button("Hide default inspector"))
                    showDefaultInspector = false;
                base.OnInspectorGUI();
            }
            else
            {
                if (GUILayout.Button("Show default inspector"))
                    showDefaultInspector = true;
            }

        }

        public static void DrawAETimes(AnimationElement ae, AnimationStep s, bool displayNameAndType = true)
        {
            GUILayout.Space(8);
            
            float animationEndDelay = ae.animationStartDelay + ae.animationLength;
            if (displayNameAndType)
                EditorGUILayout.LabelField(ae.name+ " - " + ae.animationElementType.ToString());
            EditorGUILayout.LabelField("unhide:" + ae.unhideDelay.ToString("0.0") + 
                " start:" + ae.animationStartDelay.ToString("0.0") + 
                " length:" + ae.animationLength.ToString("0.0") + 
                " end:" + animationEndDelay.ToString("0.0") + 
                " hide:" + ae.hideDelay.ToString("0.0"));
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical();
                {
                    //EditorGUILayout.LabelField("Element visibility: S=" + ae.unhideDelay.ToString("0.0") + " E= " + ae.hideDelay.ToString("0.0"));
                    EditorGUILayout.MinMaxSlider(ref ae.unhideDelay, ref ae.hideDelay, 0, s.stepLength);
                    EditorGUILayout.BeginHorizontal();
                    {
                        ae.unhideDelay = EditorGUILayout.DelayedFloatField(ae.unhideDelay);
                        ae.hideDelay = EditorGUILayout.DelayedFloatField(ae.hideDelay);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                {
                    //EditorGUILayout.LabelField("Animation duration: S=" + ae.animationStartDelay.ToString("0.0") + " L= " + ae.animationLength.ToString("0.0") + " E= " + animationEndDelay.ToString("0.0"));
                    EditorGUILayout.MinMaxSlider(ref ae.animationStartDelay, ref animationEndDelay, 0, s.stepLength);
                    EditorGUILayout.BeginHorizontal();
                    {
                        ae.animationStartDelay = EditorGUILayout.DelayedFloatField(ae.animationStartDelay);
                        animationEndDelay = EditorGUILayout.DelayedFloatField(animationEndDelay);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();


                ae.animationLength = animationEndDelay - ae.animationStartDelay;
            }
            EditorGUILayout.EndHorizontal();

        }

        void ResetObjects()
        {
            AnimationStep step = (AnimationStep)target;
            step.ResetInEditor();
        }

        void Unload()
        {
            AnimationStep step = (AnimationStep)target;
            step.KillInEditor();
        }

        void NewElement()
        {
            AnimationStep step = (AnimationStep)target;
            Selection.activeGameObject = AnimationElement.CreateEmpty(step, AnimationHolderEditor.aeName).gameObject;
            AnimationHolderEditor.InitLists();
        }

        void SaveCameraPosition()
        {
            AnimationStep step = (AnimationStep)target;
            AnimationHolder holder = step.transform.parent.GetComponent<AnimationHolder>();
            Transform t = holder.cam.transform;
            t.position = SceneView.lastActiveSceneView.camera.transform.position;
            t.eulerAngles = SceneView.lastActiveSceneView.camera.transform.eulerAngles;
            step.cameraPosition = new TransformData();
            step.cameraPosition.position = t.position;
            step.cameraPosition.eulerAngles = t.eulerAngles;
            step.cameraPosition.localScale = t.localScale;
        }

        void LoadCameraPosition()
        {
            AnimationStep step = (AnimationStep)target;
            AnimationHolder holder = step.transform.parent.GetComponent<AnimationHolder>();
            step.cameraPosition.ApplyWorld(holder.cam);
            SceneView.lastActiveSceneView.AlignViewToObject(holder.cam.transform);

        }

    }


}