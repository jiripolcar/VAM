using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Animations
{
    public partial class AnimationHolder : MonoBehaviour
    {
        public void Play()
        {
            if (!isInAnimationLoop)
            {
                if (PlayingStepID < 0)
                    PlayingStepID = 0;
                StartCoroutine(AnimationLoop());
            }
            timeScale = 1;
        }

        public void GoTo(int step)
        {
            if (PlayingStepID == step || step >= animationSteps.Count)
                return;
            else if (step < 0)
                Stop();
            else
            {
                PlayingStepID = step;
                if (!isInAnimationLoop)
                    Play();
            }
        }

        public void Stop()
        {
            PlayingStepID = -1;
        }

        public void Pause()
        {
            timeScale = 0;
        }

        public void NextStep()
        {

            if (PlayingStepID + 1 >= animationSteps.Count)
                PlayingStepID = 0;
            else
                PlayingStepID++;
        }

        public void NextStepOrPlayFirst()
        {
            if (!isInAnimationLoop)
                GoTo(0);
            else
                NextStep();

        }

        public void PreviousStep()
        {

            if (PlayingStepID - 1 < 0)
                PlayingStepID = animationSteps.Count - 1;
            else
                PlayingStepID--;
        }

        public void FirstStep()
        {
            PlayingStepID = 0;
        }
    }
}