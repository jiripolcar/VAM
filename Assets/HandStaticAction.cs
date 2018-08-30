using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandStaticAction : AnimationAction
{

    public HandAction handAction;
    public Animator handAnimator;

    private void Start()
    {
        handAnimator.SetFloat("Variant", (float)handAction);
    }

    private void Reset()
    {
        handAnimator = GetComponent<Animator>();
    }

}
