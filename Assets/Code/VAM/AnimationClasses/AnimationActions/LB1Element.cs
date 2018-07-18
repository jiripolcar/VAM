using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LB1Element : MonoBehaviour
{

    [SerializeField] public float bendingAngle = 10;
    [SerializeField] public float yawAngle = 0;
    [SerializeField] public float defaultAngle = 0;
    [SerializeField] public float startTime = 1;
    [SerializeField] public float endTime = 3;

    Vector3 localForward { get { return Vector3.up; } }

    //return forwardIsY ? Vector3.up: Vector3.forward; } }

    internal void Lerp(float stepTime)
    {
        float lerp = (stepTime - startTime) / endTime;
        if (lerp < 0)
            transform.localEulerAngles = Vector3.right * defaultAngle + localForward * yawAngle;
        else if (lerp > 1)
            transform.localEulerAngles = Vector3.right * (bendingAngle - defaultAngle) + localForward * yawAngle;
        else
            transform.localEulerAngles = Vector3.right * (lerp * bendingAngle - defaultAngle) + localForward * yawAngle;
    }

#if UNITY_EDITOR
    public GameObject prefab;
    public GameObject joint;
    public GameObject segment;
    public float segmentLengthInDefaultScale = 1;
    public float segmentCenterOffset = 0.5f;
    public bool forwardIsY;
    public bool autoRepositionChildSegments = true;
    public List<LB1Element> childSegments = new List<LB1Element>();

    public GameObject AppendSegment()
    {
        GameObject newSegment = GameObject.Instantiate(gameObject);
        newSegment.transform.position =
            transform.position + Length *
            (
                forwardIsY
                ?
                    segment.transform.up
                :
                    segment.transform.forward
            );
        newSegment.transform.SetParent(transform);
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

    public float Length
    {
        get
        {
            return (forwardIsY ? segment.transform.localScale.y : segment.transform.localScale.z) * segmentLengthInDefaultScale;
        }
        set
        {
            Vector3 spos = segment.transform.localPosition;
            Vector3 ls = segment.transform.localScale;
            if (forwardIsY)
            {
                ls.y = value / segmentLengthInDefaultScale;

            }
            else
            {
                ls.z = value / segmentLengthInDefaultScale;
            }
            segment.transform.localScale = ls;
            segment.transform.localPosition = spos;
            segment.transform.position = transform.position + transform.forward * Length / segmentLengthInDefaultScale * segmentCenterOffset;
            if (autoRepositionChildSegments)
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
