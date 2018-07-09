using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Animations
{

    [CustomEditor(typeof(AnimationStep))]
    public class AnimationStepEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            AnimationStep step = (AnimationStep)target;

            GUILayout.TextArea(
                "Place static step objects under the AR and Non-AR corresponding Animation Object Parent GameObjects.\n" +
                "If you intend using AR, the Non-AR objects will be hidden when tracking is found. If you don't intend to, place everything under the AR GameObject.\n" +
                "Also, align the Scene View camera to save the camera position. Check the Game View if the alignment is correct.\n" +
                "Also, set up the length of the step."

                );

            if (GUILayout.Button("Setup Camera")) SetupCamera();
            if (GUILayout.Button("Position Camera")) PositionCamera() ;
            if (GUILayout.Button("Setup Static Objects")) step.SetupStaticObjects();

            GUILayout.TextArea("After you've aligned the static objects, create animation elements.");
            if (GUILayout.Button("Create AnimationElement")) NewElement();

            GUILayout.TextArea("You can also load and unload all objects in this step");
            if (GUILayout.Button("Reload objects")) ResetObjects();
            if (GUILayout.Button("Unload objects")) Unload();
            
            base.OnInspectorGUI();
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

        void SetupCamera()
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

        void PositionCamera()
        {
            AnimationStep step = (AnimationStep)target;
            AnimationHolder holder = step.transform.parent.GetComponent<AnimationHolder>();
            step.cameraPosition.ApplyWorld(holder.cam);
            SceneView.lastActiveSceneView.AlignViewToObject(holder.cam.transform);
            
        }

    }


}