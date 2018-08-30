using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LB1 : AnimationAction
{
    [SerializeField] public List<LB1Element> lineBenderElements;

    protected override void SetLerpTranslated(float lerp)
    {
        lineBenderElements.ForEach((element) => element.Lerp(lerp));
    }

    /*public override void Default()
    {
        lineBenderElements.ForEach((element) => element.transform.localRotation = element.defaultRotation);
    }*/

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
        GameObject collapsed = new GameObject("Collapsed " + name);
        Transform t = collapsed.transform;
        lineBenderElements.ForEach((lb) => {
            GameObject newJoint = Instantiate(lb.joint);
            newJoint.name = lb.joint.name;
            newJoint.transform.parent = t;
            newJoint.transform.position = lb.joint.transform.position;
            newJoint.transform.rotation = lb.joint.transform.rotation;
            GameObject newSegment = Instantiate(lb.segment);
            newSegment.name = lb.segment.name;
            newSegment.transform.parent = t;
            newSegment.transform.position = lb.segment.transform.position;
            newSegment.transform.rotation = lb.segment.transform.rotation;
        });
    }

  /*  private List<LB1Element> LB1EsInChildren(LB1Element lb1e)
    {
        List<LB1Element> l = new List<LB1Element>();
        foreach (Transform t in transform)
            if (t.GetComponent<LB1Element>())
                l.Add(t.GetComponent<LB1Element>());
        return l;
    }

    private void CollapseLB1Element(LB1Element e, Transform newParent)
    {
        e.joint.transform.parent = newParent;
        e.segment.transform.parent = newParent;
        //e.childSegments.ForEach((c) => CollapseLB1Element(c, newParent));
        e.childSegments.ForEach((c) => c.transform.parent = newParent);
       // DestroyImmediate(e.gameObject);
    }*/

    public void MultipleSet(int viewStart, int viewEnd, float startTime, float endTime)
    {
        for (int i = viewStart; i < viewEnd && i < lineBenderElements.Count; i++)
        {
            lineBenderElements[i].startLerp = startTime;
            lineBenderElements[i].endLerp = endTime;
        }

    }

    public void MultipleSetMaterial(int viewStart, int viewEnd, Material material)
    {
        for (int i = viewStart; i < viewEnd && i < lineBenderElements.Count; i++)
        {
            List<Renderer> rends = new List<Renderer>();
            rends.AddRange(lineBenderElements[i].segment.GetComponentsInChildren<Renderer>());
            rends.AddRange(lineBenderElements[i].joint.GetComponentsInChildren<Renderer>());
            rends.ForEach((r) => r.material = material);
        }
    }



#endif

}
