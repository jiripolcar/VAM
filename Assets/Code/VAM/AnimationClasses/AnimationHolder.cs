using ConsoleLog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    public partial class AnimationHolder : MonoBehaviour
    {
        [SerializeField] private int playingStepID = -1;
        public int PlayingStepID { get { return playingStepID; }
            set
            {
                playingStepID = value;
                Log.AddTime(playingStepID, currentStepTime);
                currentStepTime = 0;
            }
        }
        public float timeScale = 1;
        public GameObject animationObjectsParentNonAR;
        public GameObject animationObjectsParentAR;
        public GameObject hiddenObjectsParent;
        public GameObject cam;

        [SerializeField] internal AnimationCarrier animationCarrier;
        [SerializeField] private List<AnimationStep> animationSteps;
//        [SerializeField] internal uint stepsPlayed = 0;
        private float currentStepTime = 0;
        
        public int GetStepCount()
        {
            return animationSteps.Count;
        }

        public AnimationStep CurrentStep
        {
            get
            {
                if (PlayingStepID < 0)
                    return null;
                return animationSteps[PlayingStepID];
            }
        }

        private void Start()
        {
            if (PlayingStepID >= 0)
                Play();
        }

        internal bool isInAnimationLoop = false;
        private IEnumerator AnimationLoop()
        {
            if (isInAnimationLoop || PlayingStepID < 0)
                yield break;
            isInAnimationLoop = true;

            while (PlayingStepID >= 0)
            {

                yield return StartCoroutine(animationSteps[PlayingStepID].PlayStep(PlayingStepID));
                
            }
            isInAnimationLoop = false;
        }

        private IEnumerator positionCameraCoroutineInstance;
        internal void PositionCamera()
        {
            StartCoroutine(PositioningCamera(animationSteps[PlayingStepID], 0.5f));
        }
        internal void CancelPositionCamera()
        {
            positioningCamera = false;
        }

        private bool positioningCamera = false;

        private IEnumerator PositioningCamera(AnimationStep step, float time)
        {
            if (positioningCamera)
                yield break;
            positioningCamera = true;
            Vector3 endPos = step.cameraPosition.position;
            Vector3 endEA = step.cameraPosition.eulerAngles;
            Vector3 startPos = cam.transform.localPosition;
            Vector3 startEA = cam.transform.localEulerAngles;

            if (time > 0)
            {
                float lerp = 0;
                while (lerp < 1 && positioningCamera)
                {
                    lerp += Time.deltaTime / time;
                    cam.transform.localPosition = Vector3.Lerp(startPos, endPos, lerp);
                    cam.transform.localEulerAngles = Vector3.Lerp(startEA, endEA, lerp);
                    yield return 0;
                }
            }
            cam.transform.localPosition = endPos;
            cam.transform.localEulerAngles = endEA;
            positioningCamera = false;
        }

  

        public List<string> GetStepsNames()
        {
            List<string> result = new List<string>();
            foreach (AnimationStep step in animationSteps)
                result.Add(step.name);
            return result;
        }

        private void Update()
        {
            currentStepTime += Time.deltaTime;
        }

#if UNITY_EDITOR
        public void Initialize()
        {
            animationSteps = new List<AnimationStep>();
            foreach (Transform child in transform)
            {
                AnimationStep anistep = child.gameObject.GetComponent<AnimationStep>();
                if (!anistep)
                    Debug.LogError(name + ": Object " + child.name + " is not an AnimationElementObject");
                animationSteps.Add(anistep);
                anistep.Initialize(this);
            }
        }

        public void ClearInEditor()
        {
            Initialize();
            foreach (AnimationStep s in animationSteps)
            {
                s.ClearInEditor();
            }
        }
#endif


    }
}