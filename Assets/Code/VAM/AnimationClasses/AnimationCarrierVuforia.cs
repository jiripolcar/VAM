using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    public class AnimationCarrierVuforia : AnimationCarrier
    {


        // uncomment following if Vuforia is present
        /*
        public Vuforia.TrackableBehaviour[] vuforiaTrackableBehaviours;
        private void Update()
        {
            usingAR = IsTracking;
        }

        private bool IsTracking
        {
            get
            {
                foreach (Vuforia.TrackableBehaviour vtb in vuforiaTrackableBehaviours)
                    if (vtb.CurrentStatus == Vuforia.TrackableBehaviour.Status.TRACKED || vtb.CurrentStatus == Vuforia.TrackableBehaviour.Status.EXTENDED_TRACKED)
                    {
                        return true;
                    }
                return false;
            }
        }
        */
    }
}