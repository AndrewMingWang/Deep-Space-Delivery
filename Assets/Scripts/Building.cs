using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Color SelectedColor;
    public Color PlacedColor;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.SetColor("_Color", SelectedColor);
    }

    public void setColorSelected()
    {
        meshRenderer.material.SetColor("_Color", SelectedColor);
    }

    public void setColorPlaced()
    {
        meshRenderer.material.SetColor("_Color", PlacedColor);
    }
}
