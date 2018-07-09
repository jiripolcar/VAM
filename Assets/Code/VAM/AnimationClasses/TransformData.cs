using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    [System.Serializable]
    public class TransformData
    {
        public Vector3 position = Vector3.zero;
        public Vector3 eulerAngles = Vector3.zero;
        public Vector3 localScale = Vector3.one;

        public void ApplyLocal(GameObject to)
        {
            to.transform.localEulerAngles = eulerAngles;
            to.transform.localPosition = position;
            to.transform.localScale = localScale;
        }

        public void ApplyPositionEulerAngles(GameObject to)
        {
            to.transform.localEulerAngles = eulerAngles;
            to.transform.localPosition = position;
        }

        public static TransformData GetLocal(GameObject from)
        {
            TransformData td = new TransformData();
            td.position = from.transform.localPosition;
            td.eulerAngles = from.transform.localEulerAngles;
            td.localScale = from.transform.localScale;
            return td;
        }

        public void ApplyWorld(GameObject to)
        {
            to.transform.eulerAngles = eulerAngles;
            to.transform.position = position;
            to.transform.localScale = localScale;
        }

        public static TransformData GetWorld(GameObject from)
        {
            TransformData td = new TransformData();
            td.position = from.transform.position;
            td.eulerAngles = from.transform.eulerAngles;
            td.localScale = from.transform.localScale;
            return td;
        }
    }
}
