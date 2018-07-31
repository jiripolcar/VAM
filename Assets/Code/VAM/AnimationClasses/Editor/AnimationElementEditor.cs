using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Animations
{

    [CustomEditor(typeof(AnimationElement))]
    public class AnimationElementEditor : Editor
    {

        private AnimationElement element { get { return (AnimationElement)target; } }
        private AnimationStep step { get { return element.transform.parent.GetComponent<AnimationStep>(); } }
        private static bool showDefaultInspector = false;
        private GameObject objToInit;
        private GameObject aaToInit;
        private float animationLerpView = 0;
        private bool tryAnimation = false;
        private AnimationElementType lastType;
        private static bool showAARs = false;

        public override void OnInspectorGUI()
        {
            AnimationElement e = element;
            if (e.animationElementType != lastType)
            {
                lastType = e.animationElementType;
                e.InitializeParametersLists();
            }

            AnimationStep s = step;
            GUILayout.TextArea("Place and position objects of the Animation Element. Then, either select them and initialize them or drop them in the field.");
            if (GUILayout.Button("Init selected objects")) InitFromSelection();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Init drop:");
            objToInit = (GameObject)EditorGUILayout.ObjectField(objToInit, typeof(GameObject), true);
            if (objToInit)
                InitFromField();
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Set initial position")) e.ReinitAEOs();
            GUILayout.TextArea("After initialization, place the objects in correct positions and set up the parameters based on the selected animation type.\n" +
                "Also, set the durations of this animation element.");
            if (e.animationElementType != AnimationElementType.Empty)
                GUILayout.Label("Animation type: " + e.animationElementType.ToString());

            switch (e.animationElementType)
            {
                case AnimationElementType.Empty:
                    GUILayout.TextArea("Set up Animation Element Type");
                    e.animationElementType = (AnimationElementType)EditorGUILayout.EnumPopup((System.Enum)e.animationElementType);
                    break;

                case AnimationElementType.DelayedVisible:
                    GUILayout.TextArea("Set up the times.");
                    GUILayout.Label("Fill list of animation actions or drop one in field below.");
                    break;

                case AnimationElementType.Translate:
                    GUILayout.TextArea("Translates between two given position. Place the objects in start/end position and press corresponding buttons.");
                    if (GUILayout.Button("Set start")) e.Translate_SetStart();
                    if (GUILayout.Button("Set end")) e.Translate_SetEnd();
                    if (GUILayout.Button("Switch start & end")) e.Translate_Switch();
                    break;

                case AnimationElementType.TranslateLocal:
                    GUILayout.TextArea("Translates between start position and start position + this AnimationElement holder local position." +
                        "\nThat means, place this object in relative offset position.\n" +
                        "You can also switch the positions.");
                    if (GUILayout.Button("Set start")) e.Translate_SetStart();
                    if (GUILayout.Button("Switch start & end")) TranslateLocal_Switch();
                    break;

                case AnimationElementType.TranslateUp:
                    GUILayout.TextArea("Translates in direction of this Animation Element with the speed of parameters[0].");
                    EditorGUILayout.LabelField("Set speed and direction (parameter[0])");
                    if (e.parameters == null || e.parameters.Length < 1) EditorGUILayout.LabelField("WARNING: Parameters not set.");
                    if (GUILayout.Button("Switch start & end")) TranslateUp_Switch();
                    break;

                case AnimationElementType.Rotate:
                    GUILayout.TextArea("Rotates around this object's origin and green axis with parameters[0] angular speed.");
                    EditorGUILayout.LabelField("Set this object's position and rotation and angular speed (parameter[0]");
                    if (e.parameters == null || e.parameters.Length < 1 || Mathf.Abs(e.parameters[0]) < float.Epsilon) EditorGUILayout.LabelField("Warning: Parameters[0] not set.");
                    break;

                case AnimationElementType.RotateLocal:
                    GUILayout.TextArea("Rotates around target object's origin in direction of this object's axis with parameters[0] angular speed.");
                    EditorGUILayout.LabelField("Set this object's rotation and angular speed (parameter[0]");
                    if (e.parameters == null || e.parameters.Length < 1 || Mathf.Abs(e.parameters[0]) < float.Epsilon) EditorGUILayout.LabelField("Warning: Parameters[0] not set.");
                    break;

                case AnimationElementType.RotateAdvanced:
                    GUILayout.TextArea("Rotates AEO around this object's origin and green axis from starting rotation final specified angle.");
                    //if (GUILayout.Button("Set start")) e.RotateAdvanced_SetStart();

                    if (e.parameters == null || e.parameters.Length < 1)
                        e.parameters = new float[1];
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Angle:");
                    e.parameters[0] = EditorGUILayout.FloatField(e.parameters[0]);
                    EditorGUILayout.EndHorizontal();
                    //|| Mathf.Abs(e.parameters[0]) < float.Epsilon) EditorGUILayout.LabelField("Warning: Parameters[0] not set.");
                    break;

                case AnimationElementType.Screw:
                    GUILayout.TextArea("Rotates around object's up axis and travels in the direction of the object's down axis.");
                    EditorGUILayout.LabelField("Place the object in the FINAL, screwed in position.");
                    if (GUILayout.Button("Set final position")) Screw_SetFinal();
                    EditorGUILayout.LabelField("You can set the start position by computing the screwing in movement or compute the parameters based on speed and initial - final positions difference.");
                    if (GUILayout.Button("Compute start")) Screw_ComputeStart();
                    if (GUILayout.Button("Compute parameters")) Screw_ComputeParams();
                    break;


                default:
                    EditorGUILayout.LabelField("Sorry, not implemented yet.");
                    break;
            }

            AnimationStepEditor.DrawAETimes(e, step, false);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("To init position on step start");
            e.positionOnReset = EditorGUILayout.Toggle(e.positionOnReset);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("To init position on unhide");
            e.toInitialPositionOnUnhide = EditorGUILayout.Toggle(e.toInitialPositionOnUnhide);
            EditorGUILayout.EndHorizontal();
            /*
            EditorGUILayout.LabelField("");
            EditorGUILayout.LabelField("Element visibility: S=" + e.unhideDelay.ToString("0.0") + " E= " + e.hideDelay.ToString("0.0"));
            EditorGUILayout.MinMaxSlider(ref e.unhideDelay, ref e.hideDelay, 0, s.stepLength);
            EditorGUILayout.LabelField("Animation duration: S=" + e.animationStartDelay.ToString("0.0") + " L= " + e.animationLength.ToString("0.0") + " E= " + animationEndDelay.ToString("0.0"));
            animationEndDelay = e.animationStartDelay + e.animationLength;
            EditorGUILayout.MinMaxSlider(ref e.animationStartDelay, ref animationEndDelay, 0, s.stepLength);
            e.animationLength = animationEndDelay - e.animationStartDelay;
            EditorGUILayout.Space();
            */

            EditorGUILayout.LabelField("Preview:");
            if (GUILayout.Button(tryAnimation ? "Viewing animation" : "Viewing init/start/end poses"))
            {
                tryAnimation = !tryAnimation;
                if (!tryAnimation)
                    e.Hide();
            }
            if (tryAnimation)
            {
                animationLerpView = EditorGUILayout.Slider(animationLerpView, 0, s.stepLength);
                TimePoint(animationLerpView, s, e);
            }
            else
            {
                if (GUILayout.Button("Instantiate & preview Init")) e.ReInstantiate();
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Preview start")) e.SetLerp(0);
                    if (GUILayout.Button("Preview end")) e.SetLerp(1);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Space();

            GUILayout.Label("Drop to init Animation Action");
            /*EditorGUILayout.BeginHorizontal();
            {*/
            aaToInit = (GameObject)EditorGUILayout.ObjectField(aaToInit, typeof(GameObject), true);
            if (aaToInit)
            {
                AnimationAction[] aas = aaToInit.GetComponents<AnimationAction>();
                if (e.animationActions == null)
                    e.animationActions = new List<AnimationActionRunner>();
                foreach (AnimationAction aa in aas)
                {
                    e.animationActions.Add(AnimationActionRunner.FromAnimationAction(aa));
                }

            }
            aaToInit = null;
            /*if (GUILayout.Button("Clear list of Animation Actions")) e.animationActions = new List<AnimationActionRunner>();
        }
        EditorGUILayout.EndHorizontal();*/
            int actions = e.animationActions == null ? 0 : e.animationActions.Count;
            if (actions > 0 && showAARs)
            {
                if (GUILayout.Button("Hide Animation Actions"))
                    showAARs = false;


                AnimationActionRunner toDelete = null;
                e.animationActions.ForEach((aar) =>
                {
                    if (aar != null)
                    {
                        if (DrawAAR(aar, step)) toDelete = aar;
                    }
                }
                );
                if (toDelete != null)
                    e.animationActions.Remove(toDelete);
            }
            else
            {
                
                if (actions > 0)
                {
                    if (GUILayout.Button("Show Animation Actions"))
                        showAARs = true;
                }
                else
                    GUILayout.Label("Contains " + (e.animationActions == null ? "no" : e.animationActions.Count.ToString()) + " Animation Actions");
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

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

        public static void TimePoint(float time, AnimationStep s, AnimationElement e)
        {
            if (time > e.unhideDelay && time < e.hideDelay)
            {
                e.ReInstantiate();
                if (time > e.animationStartDelay && time < e.animationStartDelay + e.animationLength)
                    e.SetLerp((time - e.animationStartDelay) / e.animationLength);
                else if (time < e.animationStartDelay)
                    e.SetLerp(0);
                else
                    e.SetLerp(1);
            }
            else
                e.Hide();
        }

        private float animationEndDelay;
        /*{
            get { return element.animationStartDelay + element.animationLength; }
            set { element.animationLength = value - element.animationStartDelay; }
        }*/

        void InitFromSelection()
        {
            GameObject[] objs = Selection.gameObjects;
            element.InitObjects(objs);
            AnimationHolderEditor.InitLists();
        }

        void InitFromField()
        {
            element.InitObjects(new GameObject[] { objToInit });
            objToInit = null;
            AnimationHolderEditor.InitLists();
        }


        void Screw_SetFinal()
        {
            element.Screw_SetFinal();
        }

        void Screw_ComputeStart()
        {
            element.Screw_ComputeStart();
        }

        void Screw_ComputeParams()
        {
            element.Screw_ComputeParams();
        }

        void TranslateLocal_Switch()
        {
            element.TranslateLocal_Switch(element.transform.localPosition);
            element.transform.localPosition *= -1;
        }

        void TranslateUp_Switch()
        {
            element.transform.localEulerAngles += new Vector3(180, 0, 0);
            element.TranslateUp_Switch();
        }

        /// <summary>
        /// Draw Animation Action Runner editor
        /// </summary>
        /// <param name="aar"></param>
        /// <param name="s"></param>
        /// <returns>Whether "delete" was pressed</returns>
        bool DrawAAR(AnimationActionRunner aar, AnimationStep s)
        {

            //EditorGUILayout.LabelField("Element visibility: S=" + ae.unhideDelay.ToString("0.0") + " E= " + ae.hideDelay.ToString("0.0"));
            GUILayout.Space(8);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(aar.animationAction.name);
            bool delete = GUILayout.Button("Delete");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Repeating");
            aar.repeating = EditorGUILayout.Toggle(aar.repeating);
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical();
                GUILayout.Label("Start");
                aar.startTime = EditorGUILayout.DelayedFloatField(aar.startTime);

                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical();
                GUILayout.Label("Length");
                aar.SetLength(EditorGUILayout.DelayedFloatField(aar.length));
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical();
                GUILayout.Label("End");
                aar.SetEndTime(EditorGUILayout.DelayedFloatField(aar.endTime));
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
            return delete;

        }

    }
}