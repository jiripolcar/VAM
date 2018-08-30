using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HandAction : int
{
    Point = 1,
    Fist = -1
}

public class HandAnimationAction : AnimationAction
{

    public HandAction handAction;
    public Animator handAnimator;

    public float maxAmount = 1;


    protected override void SetLerpTranslated(float lerp)
    {
        handAnimator.SetFloat("Variant", lerp * (int)handAction * maxAmount);
    }

    private void Reset()
    {
        handAnimator = GetComponent<Animator>();
    }

}
