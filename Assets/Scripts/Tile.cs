using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Appearance")]
    public Color BaseColor;
    public Color HoverColor;
    public Color UnbuildableColor;
    public GameObject Leaves;

    /*
    private const float r = (float)255.0f / 255.0f;
    private const float g = (float)135.0f / 255.0f;
    private const float b = (float)150.0f / 255.0f;
    */

    private Material _tileMaterial;


    [Header("Flags")]
    public bool Hovered = false;
    public bool EnvOccupied = false;
    public bool UnbuildableShadingOn = false;
    public bool EnemyTile = false;
    public bool windTile = false;
    public bool AddToTileManagerOnAwake = true;

    [Header("Objects")]
    public GameObject OccupyingBuilding = null;
    public GameObject Top;

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

    private void Start()
    {
        if (EnemyTile)
        {
            TileManager.Instance.AddUnoccupiedTile(this);
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
