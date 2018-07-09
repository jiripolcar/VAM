using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    public class AnimationElementObject : MonoBehaviour
    {
        public float delayVisible = -1;
        public float delayHide = 100;
        public AnimationObject obj;
        public Vector3[] parameters;


#if UNITY_EDITOR

        public static AnimationElementObject CreateFrom(GameObject o, AnimationElement ae, GameObject root)
        {
            AnimationElementObject aeo = new GameObject().AddComponent<AnimationElementObject>();
            aeo.transform.parent = ae.transform;
            aeo.name = o.name + ".aeo";
            aeo.transform.localPosition = Vector3.zero;
            aeo.transform.localScale = Vector3.one;
            aeo.obj = AnimationObject.CreateFromLocal(o, root);
            return aeo;
        }
#endif
    }


    public static class ListOfAnimationElementObjectExtension
    {        
        public static void HideAll(this List<AnimationElementObject> list)
        {
            foreach (AnimationElementObject aeo in list)
                aeo.obj.Hide();
        }

        public static void ResetAll(this List<AnimationElementObject> list)
        {
            foreach (AnimationElementObject aeo in list)
                aeo.obj.ResetPosition();
        }

        public static void UnhideAll(this List<AnimationElementObject> list)
        {
            foreach (AnimationElementObject aeo in list)
                aeo.obj.Unhide();
        }

#if UNITY_EDITOR
        public static AnimationElementObject Find(this List<AnimationElementObject> list, GameObject o)
        {
            if (list.Count == 0)
                return null;
            foreach (AnimationElementObject aeo in list)
            {
                if (!aeo.obj.instance)
                {
                    Debug.LogWarning(aeo.name + " this AOE has no GameObject!");
                    continue;
                }
                if (o == aeo.obj.instance)
                    return aeo;
            }
            return null;
        }
#endif
    }

}



