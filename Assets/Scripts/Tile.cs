using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Color BaseColor;
    public Color HoverColor;
    public bool Hovered = false;
    public GameObject OccupyingBuilding = null;
    private Material _tileMaterial;

    private void Awake()
    {
        _tileMaterial = GetComponent<Renderer>().material;
        _tileMaterial.SetColor("_Color", BaseColor);
    }

    public void SetHoverColor()
    {
        _tileMaterial.SetColor("_Color", HoverColor);
    }

    public void SetBaseColor()
    {
        _tileMaterial.SetColor("_Color", BaseColor);
    }
}
