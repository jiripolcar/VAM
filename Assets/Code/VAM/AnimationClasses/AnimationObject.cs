using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    [System.Serializable]
    public class AnimationObject
    {
        private AnimationHolder _animationHolder;
        private AnimationHolder animationHolder
        {
            get
            {
                if (!_animationHolder)
                    _animationHolder = rootParent.transform.parent.gameObject.GetComponentInChildren<AnimationHolder>();
                return _animationHolder;
            }
        }
        public GameObject rootParent;
        [SerializeField] private GameObject _instance;
        public GameObject instance
        {
            get
            {
                /*                if (_instance.transform.parent != rootParent.transform)
                                    _instance.transform.parent = rootParent.transform;*/
                return _instance;
                /*
                if (_instance == null)
                {
                    _instance = MakeInstance();
                    ApplyLocal();
                }
                return _instance;*/
            }
        }


        public Vector3 position;
        public Vector3 eulerAngles;
        public Vector3 scale;

        /*private GameObject MakeInstance()
        {
            GameObject n = GameObject.Instantiate(prefab);
            n.name = prefab.name;
            return n;
        }*/


        internal void Hide()
        {
            _instance.transform.parent = animationHolder.hiddenObjectsParent.transform;
            /*if (Application.isPlaying)
                GameObject.Destroy(_instance);
            else
                GameObject.DestroyImmediate(_instance);
            _instance = null;*/
        }

        internal void Unhide()
        {
            _instance.transform.parent = rootParent.transform;
        }

        private void ApplyLocal()
        {
            Transform t = _instance.transform;
            t.transform.localPosition = position;
            t.transform.localEulerAngles = eulerAngles;
            t.transform.localScale = scale;
        }

        internal void ResetPosition()
        {
            ApplyLocal();
        }


#if UNITY_EDITOR
        public static AnimationObject CreateFromLocal(GameObject target, GameObject rootParent)
        {
            AnimationObject ao = new AnimationObject();
            ao.position = target.transform.position;
            ao._instance = target;
            ao.rootParent = rootParent;
            ao.eulerAngles = target.transform.localEulerAngles;
            ao.scale = target.transform.localScale;
            return ao;
        }

        public AnimationObject ReCreate()
        {
            if (_instance)
            {
                rootParent = _instance.transform.parent.gameObject;
                position = _instance.transform.position;
                eulerAngles = _instance.transform.localEulerAngles;
                scale = _instance.transform.localScale;
            }
            return this;

        }
#endif

    }

    internal static class ListOfAnimationObjectExtension
    {
        /*public static bool ContainsGameObject(this List<AnimationObject> asos, GameObject gameObject)
        {
            foreach (AnimationObject aso in asos)
                if (gameObject == aso.prefab)
                    return true;
            return false;
        }*/

        internal static void HideAll(this List<AnimationObject> asos)
        {
            foreach (AnimationObject aso in asos)
                aso.Hide();
        }

        internal static void UnhideAll(this List<AnimationObject> asos)
        {
            foreach (AnimationObject aso in asos)
                aso.Unhide();
        }

        internal static void ResetAll(this List<AnimationObject> asos)
        {
            foreach (AnimationObject aso in asos)
                aso.ResetPosition();
        }
        /*internal static void SetActiveAll(this List<AnimationObject> asos, bool active)
        {
            foreach (AnimationObject aso in asos)
                aso.instance.SetActive(active);
        }*/

    }


}