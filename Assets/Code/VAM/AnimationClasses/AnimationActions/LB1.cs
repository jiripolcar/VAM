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
        lineBenderElements.ForEach((element) => element.transform.localRotation = element.defaultRotation);
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

    public void Collapse()
    {
        GameObject collapsed = GameObject.Instantiate(gameObject);
        collapsed.name = "Collapsed " + name;
        DestroyImmediate(collapsed.GetComponent<LB1>());

        Transform parent = collapsed.transform;

        foreach(Transform t in parent)
        {
            LB1Element lb1e = t.GetComponent<LB1Element>();
            if (lb1e!=null)
            {
                CollapseLB1Element(lb1e, parent);
            }
        }

    }

    private void CollapseLB1Element(LB1Element e, Transform newParent)
    {
        e.joint.transform.parent = newParent;
        e.segment.transform.parent = newParent;
        e.childSegments.ForEach((c) => CollapseLB1Element(c, newParent));
        DestroyImmediate(e.gameObject);
    }


#endif

}
