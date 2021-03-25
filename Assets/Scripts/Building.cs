using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public float SpawnHeight;
    public Color SelectedColor;
    public Color PlacedColor;
    public string BuildingName;
    public Tile TileUnder;
    
    protected MeshRenderer meshRenderer;

    public virtual void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.SetColor("_Color", SelectedColor);
    }

    public virtual void setColorPickedUp()
    {
        meshRenderer.material.SetColor("_Color", SelectedColor);
    }

    public virtual void setColorPlaced()
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
