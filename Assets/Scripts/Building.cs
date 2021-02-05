using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Color SelectedColor;
    public Color PlacedColor;
    public string BuildingName;
    public Tile TileUnder;
    
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.SetColor("_Color", SelectedColor);
    }

    public void setColorPickedUp()
    {
        meshRenderer.material.SetColor("_Color", SelectedColor);
    }

    public void setColorPlaced()
    {
        meshRenderer.material.SetColor("_Color", PlacedColor);
    }

    public virtual void PlaceBuilding()
    {
        setColorPlaced();
    }

    public virtual void PickUpBuilding()
    {
        setColorPickedUp();
    }
}
