using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    public class AnimationCarrier : MonoBehaviour
    {
        //private static AnimationCarrier instance;

        [SerializeField] protected bool usingAR = false;
        [SerializeField] protected AnimationHolder animationHolder;

        public virtual bool UsingAR
        {
            get { return usingAR; }
            set
            {
                if (usingAR != value)
                {
                    usingAR = value;
                    if (usingAR)
                    {
                        animationHolder.CancelPositionCamera();
                        animationHolder.animationObjectsParentNonAR.SetActive(false);
                    }
                    else
                    {
                        animationHolder.PositionCamera();
                        animationHolder.animationObjectsParentNonAR.SetActive(true);
                    }                    
                }
            }
        }

    }
}
