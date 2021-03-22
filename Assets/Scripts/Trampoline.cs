using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : Building
{
    [Header("Pickup and Place")]
    public MeshRenderer TarpMeshRenderer;

    public override void Awake()
    {
        TarpMeshRenderer.material.SetColor("_Color", SelectedColor);
    }

    public override void setColorPickedUp()
    {
        TarpMeshRenderer.material.SetColor("_Color", SelectedColor);
    }

    public override void setColorPlaced()
    {
        TarpMeshRenderer.material.SetColor("_Color", PlacedColor);
    }
}
