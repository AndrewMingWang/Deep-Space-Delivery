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
    private GameObject _leftSide;
    private GameObject _topSide;
    private GameObject _rightSide;
    private GameObject _bottomSide;

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

    [Header("Sides")]
    public bool useSides = false;
    public bool showLeft = false;
    public bool showTop = false;
    public bool showRight = false;
    public bool showBottom = false;
    public Material sidesMaterial;


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

        if (useSides){
            _leftSide = transform.GetChild(2).gameObject;
            if (showLeft) {
                _leftSide.SetActive(true);
                _leftSide.GetComponent<Renderer>().material = sidesMaterial;
            }
            _topSide = transform.GetChild(3).gameObject;
            if (showTop) {
                _topSide.SetActive(true);
                _topSide.GetComponent<Renderer>().material = sidesMaterial;
            }
            _rightSide = transform.GetChild(4).gameObject;
            if (showRight) {
                _rightSide.SetActive(true);
                _rightSide.GetComponent<Renderer>().material = sidesMaterial;

            }
            _bottomSide = transform.GetChild(5).gameObject;
            if (showBottom) {
                _bottomSide.SetActive(true);
                _bottomSide.GetComponent<Renderer>().material = sidesMaterial;

            }
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
