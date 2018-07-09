using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Animations
{

    [CustomEditor(typeof(AnimationHolder))]
    public class AnimationHolderEditor : Editor
    {
        //internal static bool drawHelp = false;

        public override void OnInspectorGUI()
        {
            //if (GUILayout.Button((drawHelp ? "Hide" : "Show") + " help")) drawHelp = !drawHelp;

            GUILayout.TextArea("Setup the necessary hierarchy. Automatically done if created via menu.");
            if (GUILayout.Button("Setup Animation")) Setup();
            GUILayout.TextArea("Check whether camera is set up correctly.");
            GUILayout.TextArea("Now create all the animation steps one by one.");
            if (GUILayout.Button("Create AnimationStep")) NewStep();
            GUILayout.TextArea("When you're done or change the hierarchy of the animations, Initialize the animation lists from the menu or with this button.");
            if (GUILayout.Button("Reinitialize Lists")) InitLists();
            GUILayout.TextArea("Delete objects before publishing!");
            if (GUILayout.Button("Clear project")) Clear();
            GUILayout.Label(" ");

            base.OnInspectorGUI();
        }


        public const string aopARName = "AnimationObjectsParentAR";
        public const string aopNARName = "AnimationStaticObjectsParentNonAR";
        public const string parentName = "AnimationParent";
        public const string aeName = "AnimationElement";
        public static  string aeoName = "AnimationElementObject";
        public const string ahName = "AnimationHolder";
        public const string defStepName = "AnimationStep";

        void NewStep()
        {
            AnimationHolder ah = (AnimationHolder)target;
            Selection.activeGameObject = AnimationStep.CreateEmpty(ah, defStepName).gameObject;
            InitLists();
        }

        internal void Setup()
        {
            Setup((AnimationHolder)target);
        }

        static void Setup(AnimationHolder ah)
        {
            ah.name = ahName;
            if (!ah.transform.parent || !ah.transform.parent.GetComponent<AnimationCarrier>())
            {
                ah.transform.parent = new GameObject(parentName).AddComponent<AnimationCarrier>().transform;
                ah.transform.parent.position = Vector3.zero;
                ah.transform.parent.eulerAngles = Vector3.zero;
            }
            ah.animationObjectsParentNonAR = GameObject.Find(aopNARName);
            if (!ah.animationObjectsParentNonAR)
                ah.animationObjectsParentNonAR = new GameObject(aopNARName);

            ah.animationObjectsParentAR = GameObject.Find(aopARName);
            if (!ah.animationObjectsParentAR)
                ah.animationObjectsParentAR = new GameObject(aopARName);

            Transform aopn = ah.animationObjectsParentNonAR.transform;
            Transform aop = ah.animationObjectsParentAR.transform;
            ah.transform.position = aopn.position = aop.position = Vector3.zero;
            ah.transform.eulerAngles = aopn.eulerAngles = aop.eulerAngles = Vector3.zero;
            ah.transform.localScale = aopn.localScale = aop.localScale = Vector3.one;
            aop.transform.parent = aopn.transform.parent = ah.transform.parent;
            ah.cam = Camera.main.gameObject;
        }

        [MenuItem("V Assy Man/Setup")]
        private static void SetupVAssyMan()
        {
            /*
            AnimationHolder ah = FindOrCreateAH();
            Setup(ah);
            Selection.activeGameObject = ah.gameObject;*/
            GameObject ap = GameObject.Find(parentName);
            if (!ap)
            {
                ap = Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets/VAssyMan/Prefabs/AnimationParent.prefab", typeof(GameObject)));
                ap.name = parentName;                
                Setup(GameObject.Find(ahName).GetComponent<AnimationHolder>());
            }
            Selection.activeGameObject = ap;
        }

        [MenuItem("V Assy Man/Initialize lists")]
        internal static void InitLists()
        {
            if (GameObject.Find(ahName))
                GameObject.Find(ahName).GetComponent<AnimationHolder>().Initialize();
            else
                Debug.LogError("Could not initialize the lists. Was the AnimationHolder GameObject renamed?");
        }

        [MenuItem("V Assy Man/Clear objects")]
        static public void Clear()
        {
            AnimationHolder ah = FindOrCreateAH();
            if (ah)
                ah.ClearInEditor();
            /*
            for (int i = ah.animationObjectsParentAR.transform.childCount; i >= 0; i--)
                DestroyImmediate(ah.animationObjectsParentAR.transform.GetChild(i));

            for (int i = ah.animationObjectsParentNonAR.transform.childCount; i >= 0; i--)
                DestroyImmediate(ah.animationObjectsParentNonAR.transform.GetChild(i));*/
        }

        static AnimationHolder FindOrCreateAH()
        {
            GameObject aho = GameObject.Find(ahName);
            AnimationHolder ah;
            if (aho)
                ah = aho.GetComponent<AnimationHolder>();
            else
                ah = new GameObject().AddComponent<AnimationHolder>();
            return ah;
        }
    }


}