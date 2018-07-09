using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    public enum StepEndAction
    {
        Loop,
        End,
        Next
    }

    public class AnimationStep : MonoBehaviour
    {
        public float stepLength = 5;
        public StepEndAction stepEnd;

        [SerializeField] private List<AnimationElement> animationElements = new List<AnimationElement>();
        [SerializeField] internal AnimationHolder holder;

        [SerializeField] private List<AnimationObject> staticAnimationObjectsAR = new List<AnimationObject>();
        [SerializeField] private List<AnimationObject> staticAnimationObjectsNonAR = new List<AnimationObject>();

        public TransformData cameraPosition;

        internal float stepTime;
        internal float deltaTime;
        private float lastTime;

        internal IEnumerator PlayStep(int playingStep)
        {
//            uint step;
            do
            {
                //step = ++holder.stepsPlayed;
                ShowStaticObjects();
                holder.PositionCamera();
                stepTime = lastTime = deltaTime = 0;                

                animationElements.ForEach((ae) => ae.ResetElement());

                while (stepTime < stepLength && playingStep == holder.PlayingStepID)
                {
                    stepTime += Time.deltaTime * holder.timeScale; 
                    deltaTime = stepTime - lastTime;
                    lastTime = stepTime;
                    animationElements.ForEach((ae) => ae.AnimateElement(stepTime));
                    yield return 0;
                }
                animationElements.ForEach((ae) => ae.EndElement());
            } while (stepEnd == StepEndAction.Loop && playingStep == holder.PlayingStepID);
            if (stepEnd == StepEndAction.Next && playingStep == holder.PlayingStepID)
                holder.NextStep();
            else
                while (stepEnd == StepEndAction.End && playingStep == holder.PlayingStepID)
                    yield return 0;
            HideStaticObjects();
//            step = ++holder.stepsPlayed;
        }

        internal IEnumerator PlayStepOLD(int playingStep)
        {
            //uint step;
            /*            if (!holder.animationCarrier.UsingAR)
                            holder.PositionCamera();*/
            do
            {
                //step = ++holder.stepsPlayed;
                ShowStaticObjects();
                holder.PositionCamera();
                stepTime = lastTime = deltaTime = 0;

         //       foreach (AnimationElement ae in animationElements)
       //             StartCoroutine(ae.PlayElementOLD(step));
                
                while (stepTime < stepLength && playingStep == holder.PlayingStepID)
                {
                    stepTime += Time.deltaTime * holder.timeScale;
                    deltaTime = stepTime - lastTime;
                    lastTime = stepTime;
                    yield return 0;
                }
            } while (stepEnd == StepEndAction.Loop && playingStep == holder.PlayingStepID);
            if (stepEnd == StepEndAction.Next && playingStep == holder.PlayingStepID)
                holder.NextStep();
            else
                while (stepEnd == StepEndAction.End && playingStep == holder.PlayingStepID)
                    yield return 0;
            HideStaticObjects();
         //   step = ++holder.stepsPlayed;
        }

        internal void ShowStaticObjects()
        {
            staticAnimationObjectsAR.ResetAll();
            staticAnimationObjectsNonAR.ResetAll();
            staticAnimationObjectsAR.UnhideAll();
            if (holder.animationCarrier.UsingAR)
                staticAnimationObjectsNonAR.HideAll();
            else
                staticAnimationObjectsNonAR.UnhideAll();
        }

        internal void HideStaticObjects()
        {
            staticAnimationObjectsAR.HideAll();
            staticAnimationObjectsNonAR.HideAll();
        }

#if UNITY_EDITOR
        public static AnimationStep CreateEmpty(AnimationHolder holder, string name)
        {
            GameObject carrier = new GameObject();
            carrier.transform.parent = holder.transform;
            carrier.transform.localPosition = Vector3.zero;
            carrier.transform.localEulerAngles = Vector3.zero;
            carrier.transform.localScale = Vector3.one;
            carrier.name = name;
            return carrier.AddComponent<AnimationStep>();
        }

        public void ClearInEditor()
        {
            HideStaticObjects();
            foreach (AnimationElement ae in animationElements)
            {
                ae.ClearInEditor();
            }
        }

        public void ResetInEditor()
        {
            ShowStaticObjects();
            /*staticAnimationObjectsAR.SetActiveAll(true);
            staticAnimationObjectsNonAR.SetActiveAll(true);*/
        }

        public void KillInEditor()
        {
            HideStaticObjects();
        }

        public void SetupStaticObjects()
        {
            staticAnimationObjectsAR = new List<AnimationObject>();
            foreach (Transform t in holder.animationObjectsParentAR.transform)
            {
                staticAnimationObjectsAR.Add(AnimationObject.CreateFromLocal(t.gameObject, holder.animationObjectsParentAR));
            }
            staticAnimationObjectsNonAR = new List<AnimationObject>();
            foreach (Transform t in holder.animationObjectsParentNonAR.transform)
            {
                staticAnimationObjectsNonAR.Add(AnimationObject.CreateFromLocal(t.gameObject, holder.animationObjectsParentNonAR));
            }
        }

        internal void Initialize(AnimationHolder holder)
        {
            this.holder = holder;
            animationElements = new List<AnimationElement>();
            foreach (Transform child in transform)
            {
                AnimationElement ae = child.gameObject.GetComponent<AnimationElement>();
                if (!ae)
                    Debug.LogError("Object " + child.name + " is not an AnimationElement");
                animationElements.Add(ae);
                ae.Initialize(this, holder);
            }
        }

   

#endif

    }
}
