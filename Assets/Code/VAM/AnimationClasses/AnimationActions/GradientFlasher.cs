using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientFlasher : AnimationAction
{

    [SerializeField] private Gradient gradient;

    [SerializeField] public List<Material> materials;
    [SerializeField] private List<Color> defaultColors;
    [SerializeField] private float startTime = 1;
    [SerializeField] private float endTime = 3;
    [SerializeField] private GameObject parent;

    private void Reset()
    {
        parent = gameObject;
    }

#if UNITY_EDITOR
    public void FillMaterials()
    {
        materials = new List<Material>();
        defaultColors = new List<Color>();
        
        FillMaterialsFromChildren(parent.transform);
     
        materials.ForEach((m) => defaultColors.Add(m.color));
    }

    public void FillMaterialsFromChildren(Transform child)
    {
        Renderer r = child.GetComponent<Renderer>();
        if (r)
            materials.AddRange(r.sharedMaterials);            
        foreach (Transform t in child) FillMaterialsFromChildren(t);
    }
#endif

    public void SetColors(Color c)
    {
        materials.ForEach((m) => m.color = c);
    }

    public void SetDefaultColors()
    {
        for (int i = 0; i < materials.Count; i++)
        {
            materials[i].color = defaultColors[i];
        }
    }

    public override void SetTime(float stepTime)
    {
        float lerp = (stepTime - startTime) / endTime;
        print("Flasher time: " + stepTime + " lerp: " + lerp);
        if (lerp > 0 && lerp < 1)
            SetColors(gradient.Evaluate(lerp));
        else
            SetDefaultColors();
    }

    public override void Default()
    {
        SetDefaultColors();
    }
}
