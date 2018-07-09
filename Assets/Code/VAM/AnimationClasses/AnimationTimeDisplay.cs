using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    public class AnimationTimeDisplay : MonoBehaviour
    {

        public UnityEngine.UI.Text timeText;
        public AnimationHolder holder;

        // Update is called once per frame
        void Update()
        {
            AnimationStep currentStep = holder.CurrentStep;
            string text = "Step: " + holder.PlayingStepID;
            
            if (currentStep)
                text += " = " + currentStep.name + " Time: " + currentStep.stepTime.ToString("0.0");
            timeText.text = text;


        }
    }
}
