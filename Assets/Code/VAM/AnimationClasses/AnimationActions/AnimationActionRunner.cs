using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Animations
{
    [System.Serializable]
    public class AnimationActionRunner
    {
        public AnimationAction animationAction;
        public float startTime = 1;
        public float length = 2;
        public float endTime = 3;
        public bool repeating = false;

        public static AnimationActionRunner FromAnimationAction(AnimationAction aa)
        {
            AnimationActionRunner aar = new AnimationActionRunner();
            aar.animationAction = aa;
            return aar;
        }

        public void SetLength(float l)
        {
            length = l;
            if (!repeating)
            {
                endTime = length + startTime;
            }
        }

        public void SetEndTime(float et)
        {
            endTime = et;
            if (!repeating)
            {
                length = endTime - startTime;
            }
        }

        public void SetTime(float time)
        {
            float lerp = (time - startTime) / length;
            if (lerp > 0 && time < endTime)
            {
                if (repeating)
                {
                    lerp -= Mathf.Floor(lerp);
                }
                animationAction.SetLerp(lerp);
            }
            else
                animationAction.Default();
        }


    }


}