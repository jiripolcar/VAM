using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    public class ArrowKeysFrontend : MonoBehaviour
    {
        [SerializeField] private AnimationHolder animationHolder;
        [SerializeField] private KeyCode nextStep = KeyCode.RightArrow;
        [SerializeField] private KeyCode previousStep = KeyCode.LeftArrow;


        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(nextStep))
                animationHolder.NextStepOrPlayFirst();
            if (Input.GetKeyDown(previousStep))
                animationHolder.PreviousStep();
        }
    }
}