using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAction : MonoBehaviour
{
    public virtual void SetTime(float stepTime) { }
    public virtual void Default() { }

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