using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    [System.Serializable]
    public class AnimationStaticObject : TransformData
    {
        public GameObject staticObject;
        public void ApplyLocal()
        {
            ApplyLocal(staticObject);
        }
        new public static AnimationStaticObject GetLocal(GameObject from)
        {
            TransformData td = TransformData.GetLocal(from);

            AnimationStaticObject aso = new AnimationStaticObject();
            aso.position = td.position;
            aso.localScale = td.localScale;
            aso.eulerAngles = td.eulerAngles;
            aso.staticObject = from;
            return aso;
        }
    }

    public static class ListOfAnimationStaticObjectExtension
    {
        public static bool ContainsGameObject(this List<AnimationStaticObject> asos, GameObject gameObject)
        {
            foreach (AnimationStaticObject aso in asos)
                if (gameObject == aso.staticObject)
                    return true;
            return false;
        }
    }
}