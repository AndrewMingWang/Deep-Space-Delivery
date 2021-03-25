using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Color BaseColor;
    public Color HoverColor;
    public Color UnbuildableColor;

    /*
    private const float r = (float)255.0f / 255.0f;
    private const float g = (float)135.0f / 255.0f;
    private const float b = (float)150.0f / 255.0f;
    */
    public GameObject Leaves;

    [HideInInspector]
    public bool Hovered = false;

    public bool EnvOccupied = false;
    public bool UnbuildableShadingOn = false;

    public GameObject OccupyingBuilding = null;

    private Material _tileMaterial;

    public GameObject Top;
    public bool windTile = false;
    public bool AddToTileManagerOnAwake = true;

    public Tile(Color basecol, Color hovercol, GameObject top){

        BaseColor = basecol;
        HoverColor = hovercol;
        Top = top;
    }

    private void Awake()
    {
        if (windTile){
            // BaseColor = new Color(r, g, b);
            //Top.GetComponent<Renderer>().materials[0].SetColor("_Color", BaseColor);
            Leaves.SetActive(true);
        } 
        _tileMaterial = Top.GetComponent<Renderer>().materials[1];
        if (!UnbuildableShadingOn){
            _tileMaterial.SetColor("_Color", BaseColor);        
        } else {
            _tileMaterial.SetColor("_Color", UnbuildableColor);        
        }
    }

    public void SetHoverColor()
    {
        _tileMaterial.SetColor("_Color", HoverColor);
    }

    public void SetBaseColor()
    {
        if (!UnbuildableShadingOn){
            _tileMaterial.SetColor("_Color", BaseColor);
        } else {
            _tileMaterial.SetColor("_Color", UnbuildableColor);        
        }
    }
}
