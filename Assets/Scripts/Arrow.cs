using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Building
{
    public MeshRenderer SignRenderer;
    public MeshRenderer PoleRenderer;

    private void Awake()
    {
        SignRenderer.material.SetColor("_Color", SelectedColor);
        PoleRenderer.material.SetColor("_Color", SelectedColor);
    }

    public override void setColorPickedUp()
    {
        SignRenderer.material.SetColor("_Color", SelectedColor);
        PoleRenderer.material.SetColor("_Color", SelectedColor);
    }

    public override void setColorPlaced()
    {
        SignRenderer.material.SetColor("_Color", PlacedColor);
        PoleRenderer.material.SetColor("_Color", PlacedColor);
    }
}
