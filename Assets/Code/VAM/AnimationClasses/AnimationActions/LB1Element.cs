using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LB1Element : MonoBehaviour
{
    [SerializeField] public float startLerp = 0f;
    [SerializeField] public float endLerp = 1f;

    [SerializeField] public Quaternion defaultRotation = Quaternion.identity;
    [SerializeField] public Quaternion targetRotation = Quaternion.identity;

    internal void Lerp(float lerp)
    {
        lerp = (lerp - startLerp) / (endLerp - startLerp);

        if (lerp < 0)
            transform.localRotation = defaultRotation;
        else if (lerp > 1)
            transform.localRotation = targetRotation;
        else
            transform.localRotation = Quaternion.Lerp(defaultRotation, targetRotation, lerp);
    }



#if UNITY_EDITOR
    [Space(10)]
    public GameObject joint;
    public GameObject segment;
    public float segmentLengthInDefaultScale = 1;

    public List<LB1Element> childSegments = new List<LB1Element>();

    public void SaveDefaultRotation()
    {
        defaultRotation = transform.localRotation;
    }

    public void SaveTargetRotation()
    {
        targetRotation = transform.localRotation;
    }

    public void ComputeTargetRotationAndLength(GameObject targetToConnect, bool isTarget)
    {
        transform.LookAt(targetToConnect.transform);
        Length = (transform.position - targetToConnect.transform.position).magnitude;
        if (isTarget)
            targetRotation = transform.localRotation;
        else
            defaultRotation = transform.localRotation;
    }

    public GameObject AppendSegment()
    {
        GameObject newSegment = GameObject.Instantiate(gameObject);
        //newSegment.transform.position = transform.position + Length * transform.forward;
        newSegment.transform.SetParent(transform);
        newSegment.transform.localPosition = Vector3.forward * Length;
        newSegment.transform.localEulerAngles = Vector3.zero;
        childSegments.Add(newSegment.GetComponent<LB1Element>());

        int attempts = 0;
        int found = 0;
        string n;
        do
        {
            n = gameObject.name + "_" + attempts++;
            found = 0;
            foreach (Transform t in transform)
            {
                if (t.name == n)
                    found++;
            }
        } while (found > 0);
        newSegment.name = n;
        return newSegment;
    }

    public void RepositionChildSegments()
    {
        childSegments.RemoveAll((child) => child == null);
        childSegments.ForEach((child) =>
        {
            child.transform.localPosition = Vector3.forward * Length;
        });
    }

    public float GetTotalLength
    {
        get
        {
            float l = Length;
            /*if (childSegments == null || childSegments.Count == 0)
                return l;
            else
            {*/
            childSegments.ForEach((cs) => l += cs.GetTotalLength);
            return l;
            //}
        }
    }

    public float Length
    {
        get
        {
            return segment.transform.localScale.z * segmentLengthInDefaultScale;
        }
        set
        {

            Vector3 ls = segment.transform.localScale;
            ls.z = value / segmentLengthInDefaultScale;
            segment.transform.localScale = ls;
            RepositionChildSegments();
        }
    }

    public void FindInChildren(List<LB1Element> list)
    {
        list.Add(this);
        foreach (Transform t in transform)
        {
            if (t.GetComponent<LB1Element>())
                t.GetComponent<LB1Element>().FindInChildren(list);

        }
    }


    public bool IsScaleOne { get { return (transform.localScale - Vector3.one).sqrMagnitude < 0.01f; } }
#endif

}
