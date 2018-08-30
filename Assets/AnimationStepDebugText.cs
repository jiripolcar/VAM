using Animations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationStepDebugText : MonoBehaviour {

    public AnimationHolder animationHolder;
    public Text debugText;

	// Update is called once per frame
	void Update () {
        AnimationStep current = animationHolder.CurrentStep;
        debugText.text = "T: " + current.stepTime + " Step: " + current.name;
	}
}
