using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : Building
{
    [Header("Pickup and Place")]
    public GameObject Tarp;
    public List<MeshRenderer> Frames;

    public override void Awake()
    {
        Tarp.SetActive(false);
        foreach (MeshRenderer m in Frames)
        {
            m.material.SetColor("_Color", SelectedColor);
        }
    }

    public override void setColorPickedUp()
    {
        Tarp.SetActive(false);
        foreach (MeshRenderer m in Frames)
        {
            m.material.SetColor("_Color", SelectedColor);
        }
    }

    public override void setColorPlaced()
    {
        Tarp.SetActive(true);
        foreach (MeshRenderer m in Frames)
        {
            m.material.SetColor("_Color", PlacedColor);
        }
    }
}
