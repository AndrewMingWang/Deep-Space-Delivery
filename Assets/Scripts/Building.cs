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
    private int _updateTime;
    protected MeshRenderer meshRenderer;

    public virtual void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.SetColor("_Color", SelectedColor);
        _updateTime = 0;
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

    public virtual void Reset()
    {

    }

    // private void Update() {
    //     if (TileUnder != null && _updateTime == 5){
    //         transform.position = TileUnder.transform.position + SpawnHeight*transform.up;
    //         _updateTime = 0;
    //     } else {
    //         _updateTime++;
    //     }
    // }
}
