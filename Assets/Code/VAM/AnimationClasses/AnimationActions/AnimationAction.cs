﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAction : MonoBehaviour
{    
    public void SetLerp(float lerp) { SetLerpTranslated(lerpTranslationCurve.Evaluate(lerp)); }    
    public virtual void Default() { SetLerp(0); }

    protected virtual void SetLerpTranslated(float lerp) { }
    [SerializeField] protected AnimationCurve lerpTranslationCurve = AnimationCurve.Linear(0, 1, 1, 1);

    /*public virtual void Activate(float param1 = 0) { }
    public virtual void Deactivate() { }
    public virtual void SetToStart() { }*/
}

/*public static class ListOfAnimationActionsExtensions
{
    public static void ActivateAll(this List<AnimationAction> list, float param1 = 0)
    {
        foreach (AnimationAction aa in list)
            aa.Activate(param1);
    }

    public static void DeactivateAll(this List<AnimationAction> list, float param1 = 0)
    {
        foreach (AnimationAction aa in list)
            aa.Deactivate();
    }

    public static void ResetAll(this List<AnimationAction> list, float param1 = 0)
    {
        foreach (AnimationAction aa in list)
            aa.SetToStart();
    }
}*/