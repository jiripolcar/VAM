using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    public enum AnimationElementType : int
    {
        Empty = 0,
        DelayedVisible = 1,     // nothing, plays animation actions eventually
        Translate = 10,         // local = lerp(aeo.par[0] > aeo.par[1])
        TranslateLocal = 11,    // local = lerp(aeo.start > aeo.start + this.local)
        TranslateUp = 12,       // world += this.worldUp * deltaTime * this.par[0]                                
        Rotate = 20,            // rotate around (this.worldUp x this.position, this.par[0] * deltaTime)
        RotateLocal = 21,        // rotate around (this.worldUp x aeo.position, this.par[0] * deltaTime)
        Screw                   // rotate around (aeo.worldup x aeo.position, this.par[0] * deltaTime)
                                // local = lerp (aeo.start - aeo.up*length*this.par[1] > aeo.start)

    }

    public class AnimationElement : MonoBehaviour
    {
        public AnimationElementType animationElementType;
        public bool positionOnReset = true;
        public bool toInitialPositionOnUnhide = false;
        public float unhideDelay = 0.5f;
        public float animationStartDelay = 1;
        public float animationLength = 3;
        public float hideDelay = 5;
        public float[] parameters;
        public List<AnimationAction> animationActions = new List<AnimationAction>();

        [SerializeField] private List<AnimationElementObject> animationElementObjects;
        [SerializeField] private AnimationStep step;
        [SerializeField] private AnimationHolder holder;

        #region prekopani

        private bool _hidden = true;
        private bool Hidden
        {
            get { return _hidden; }
            set
            {
                _hidden = value;
                if (_hidden)
                    animationElementObjects.HideAll();
                else
                {
                    animationElementObjects.UnhideAll();
                    if (toInitialPositionOnUnhide)
                    {
                        animationElementObjects.ResetAll();
                    }
                }
            }
        }

        internal void ResetElement()
        {
            if (positionOnReset)
                animationElementObjects.ResetAll();
        }

        internal void AnimateElement(float time)
        {
            bool shouldBeHidden = time < unhideDelay || time > hideDelay;
            if (Hidden != shouldBeHidden)
                Hidden = shouldBeHidden;

            if (Hidden)
                return;
            else
            {
                float lerp = (time - animationStartDelay) / animationLength;
                if (animationActions.Count > 0)
                    animationActions.ForEach((aa) => aa.SetTime(time));
                if (lerp >= 0 && lerp <= 1)
                    animationElementObjects.ForEach((aeo) => Animate(aeo, time, lerp, step.deltaTime));
            }


        }

        internal void EndElement()
        {
            Hidden = true;
            animationActions.ForEach((aa) => aa.Default());
        }

        #endregion

        #region old

        /*    internal IEnumerator PlayElementOLD(uint stepNumber)
            {
                yield return 0;
                if (positionOnReset)
                    animationElementObjects.ResetAll();
                foreach (AnimationElementObject aeo in animationElementObjects)
                    aeo.obj.ResetPosition();
                while (step.stepTime < unhideDelay)
                    yield return 0;

                animationElementObjects.UnhideAll();
                foreach (AnimationElementObject aeo in animationElementObjects)
                    aeo.obj.Unhide();

                float lastTime = step.stepTime - animationStartDelay;
                while (stepNumber == holder.stepsPlayed)
                {
                    float t = step.stepTime - animationStartDelay;
                    if (lastTime > t || t > animationLength)
                        break;
                    lastTime = t;
                    float dT = step.deltaTime;
                    float lerp = t / animationLength;

                    if (t > 0 && lerp < 1)
                        foreach (AnimationElementObject o in animationElementObjects)
                            Animate(o, t, lerp, dT);

                    yield return 0;
                }
                lastTime = step.stepTime - animationStartDelay - animationLength;

                animationElementObjects.HideAll();
            }*/
        #endregion

        internal virtual void Animate(AnimationElementObject o, float time, float lerp, float delta)
        {
            bool visible = time > o.delayVisible && time < o.delayHide;
            if (o.obj.instance.activeInHierarchy != visible)
                o.obj.instance.SetActive(visible);
            if (!visible)
                return;

            Transform t = o.obj.instance.transform;
            /*        Empty = 0,
        DelayedVisible = 1,     // nothing
        Translate = 10,         // local = lerp(aeo.par[0] > aeo.par[1])
        TranslateLocal = 11,    // local = lerp(aeo.start (=aeo.par[0]) > aeo.start + this.local)
        TranslateUp = 12,       // world += this.worldUp * deltaTime * this.par[0]                                
        Rotate = 20,            // rotate around (this.worldUp x this.position, this.par[0] * deltaTime)
        RotateLocal = 21        // rotate around (this.worldUp x aeo.position, this.par[0] * deltaTime)*/
            switch (animationElementType)
            {
                case AnimationElementType.Translate:
                    t.localPosition = Vector3.Lerp(o.parameters[0], o.parameters[1], lerp);
                    break;

                case AnimationElementType.TranslateLocal:
                    t.localPosition = Vector3.Lerp(o.parameters[0], o.parameters[0] + transform.localPosition, lerp);
                    break;

                case AnimationElementType.TranslateUp:
                    t.position += transform.up * parameters[0] * delta;
                    break;

                case AnimationElementType.Rotate:
                    t.RotateAround(transform.position, transform.up, parameters[0] * delta);
                    break;

                case AnimationElementType.RotateLocal:
                    t.RotateAround(t.position, transform.up, parameters[0] * delta);
                    break;

                case AnimationElementType.DelayedVisible:
                    break;

                case AnimationElementType.Screw:
                    t.localPosition = Vector3.Lerp(o.parameters[0] + t.up * animationLength * parameters[1], o.parameters[0], lerp);
                    t.RotateAround(t.position, t.transform.up, parameters[0] * delta);
                    // rotate around (aeo.worldup x aeo.position, this.par[0] * deltaTime)
                    // local = lerp (aeo.start=o.parameters[0] - aeo.up*length*this.par[1] > aeo.start)
                    break;

                case AnimationElementType.Empty:
                    break;
                default:
                    Debug.LogError(name + " is a undefined AnimationElement!");
                    break;
            }
        }

#if UNITY_EDITOR
        public void InitializeParametersLists()
        {
            int floats = FloatParameters(animationElementType);
            if (parameters == null || parameters.Length < floats)
                parameters = new float[floats];
            int vectors = VectorParameters(animationElementType);
            foreach (AnimationElementObject aeo in animationElementObjects)
                if (aeo.parameters == null || aeo.parameters.Length < vectors)
                    aeo.parameters = new Vector3[vectors];
        }
        private int FloatParameters(AnimationElementType t)
        {
            switch (t)
            {
                case AnimationElementType.DelayedVisible:
                case AnimationElementType.Empty:
                case AnimationElementType.Translate:
                case AnimationElementType.TranslateLocal:
                    return 0;
                case AnimationElementType.Rotate:
                case AnimationElementType.RotateLocal:
                case AnimationElementType.TranslateUp:
                    return 1;
                case AnimationElementType.Screw:
                    return 2;
                default:
                    throw new System.Exception("Not prepared!");
            }
        }
        private int VectorParameters(AnimationElementType t)
        {
            switch (t)
            {
                case AnimationElementType.DelayedVisible:
                case AnimationElementType.Empty:
                case AnimationElementType.Rotate:
                case AnimationElementType.RotateLocal:
                case AnimationElementType.TranslateUp:
                    return 0;
                case AnimationElementType.TranslateLocal:
                case AnimationElementType.Screw:
                    return 1;
                case AnimationElementType.Translate:
                    return 2;
                default:
                    throw new System.Exception("Not prepared!");
            }
        }

        internal void Initialize(AnimationStep step, AnimationHolder holder)
        {
            this.step = step;
            this.holder = holder;
            animationElementObjects = new List<AnimationElementObject>();
            foreach (Transform child in transform)
            {
                AnimationElementObject aeo = child.gameObject.GetComponent<AnimationElementObject>();
                if (!aeo)
                    Debug.LogError(name + ": Object " + child.name + " is not an AnimationElementObject");
                animationElementObjects.Add(aeo);
            }
        }
        public static AnimationElement CreateEmpty(AnimationStep step, string name)
        {
            GameObject o = new GameObject();
            o.name = name;
            o.transform.parent = step.transform;
            o.transform.localPosition = Vector3.zero;
            o.transform.localEulerAngles = Vector3.zero;
            o.transform.localScale = Vector3.one;
            return o.AddComponent<AnimationElement>();
        }

        public void SetLerp(float lerp)
        {
            float delta = animationLength * lerp;
            foreach (AnimationElementObject o in animationElementObjects)
                Animate(o, animationStartDelay + delta, lerp, delta);
        }

        public void InitObjects(GameObject[] selection)
        {
            foreach (GameObject o in selection)
            {
                if (!(o.transform.parent == holder.animationObjectsParentAR.transform || o.transform.parent == holder.animationObjectsParentNonAR.transform))
                {
                    Debug.LogError("Error! Animated object is not a direct child of a Animation Objects Parent!");
                    return;
                }

                AnimationElementObject found = animationElementObjects.Find(o);
                if (found)
                    found.obj.ReCreate();
                else
                    animationElementObjects.Add(AnimationElementObject.CreateFrom(o, this, o.transform.parent.gameObject));

            }
        }

        public void ReinitAEOs()
        {
            foreach (AnimationElementObject aeo in animationElementObjects)
                aeo.obj.ReCreate();
        }

        public void Screw_SetFinal()
        {
            foreach (AnimationElementObject aeo in animationElementObjects)
            {
                aeo.parameters = new Vector3[1];
                aeo.parameters[0] = aeo.obj.instance.transform.localPosition;
            }
        }

        public void Screw_ComputeStart()
        {
            foreach (AnimationElementObject aeo in animationElementObjects)
            {
                aeo.obj.position = aeo.parameters[0] + aeo.obj.instance.transform.up * animationLength * parameters[1];
            }
        }

        public void Screw_ComputeParams()
        {
            foreach (AnimationElementObject aeo in animationElementObjects)
            {
                float translationLength = (aeo.parameters[0] - aeo.obj.position).magnitude;
                parameters[1] = translationLength / animationLength;
            }
        }

        public void ReInstantiate()
        {
            animationElementObjects.UnhideAll();
            animationElementObjects.ResetAll();
        }

        public void Hide()
        {
            animationElementObjects.HideAll();
        }

        public void Translate_SetStart()
        {
            foreach (AnimationElementObject aeo in animationElementObjects)
            {
                if (aeo.parameters == null || aeo.parameters.Length < 2)
                    aeo.parameters = new Vector3[2];
                aeo.parameters[0] = aeo.obj.instance.transform.localPosition;
            }
        }
        public void Translate_SetEnd()
        {
            foreach (AnimationElementObject aeo in animationElementObjects)
            {
                if (aeo.parameters == null || aeo.parameters.Length < 2)
                    aeo.parameters = new Vector3[2];
                aeo.parameters[1] = aeo.obj.instance.transform.localPosition;
            }
        }
        public void Translate_Switch()
        {
            foreach (AnimationElementObject aeo in animationElementObjects)
            {
                Vector3 h = aeo.parameters[0];
                aeo.parameters[0] = aeo.parameters[1];
                aeo.parameters[1] = h;
            }
        }

        public void TranslateLocal_Switch(Vector3 p)
        {
            foreach (AnimationElementObject aeo in animationElementObjects)
            {
                aeo.parameters[0] += p;

            }
        }

        public void TranslateUp_Switch()
        {
            Vector3 p = transform.up * parameters[0] * animationLength;
            //transform.position = p;
            //transform.LookAt(p);
            foreach (AnimationElementObject aeo in animationElementObjects)
            {
                aeo.obj.position += p;
                //aeo.transform.position -= p;
            }

        }

        public void ClearInEditor()
        {
            foreach (AnimationElementObject aeo in animationElementObjects)
            {
                aeo.obj.Hide();
            }
        }

#endif
    }
}
