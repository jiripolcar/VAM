using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LB1 : AnimationAction
{
    [SerializeField] public List<LB1Element> lineBenderElements;

    public override void SetTime(float stepTime)
    {
        lineBenderElements.ForEach((element) => element.Lerp(stepTime));
    }

    public override void Default()
    {
        lineBenderElements.ForEach((element) => element.transform.localEulerAngles = Vector3.right * element.defaultAngle);
    }

    private void Reset()
    {
        lineBenderElements = new List<LB1Element>();
    }

#if UNITY_EDITOR
    public void FillLineBenderElementsList()
    {
        lineBenderElements = new List<LB1Element>();
        foreach (Transform t in transform)
            if (t.GetComponent<LB1Element>())
                t.GetComponent<LB1Element>().FindInChildren(lineBenderElements);
    }


#endif

}
