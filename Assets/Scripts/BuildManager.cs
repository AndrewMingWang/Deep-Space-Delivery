﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    public static bool BuildingSelected = false;

    public Building CurrBuilding;

    // "wall" - Wall prefab
    // "arrow" - Arrow prefab
    [HideInInspector]
    public const string WALL = "wall";
    [HideInInspector]
    public const string ARROW = "arrow";
    [HideInInspector]
    public const string HOLDING = "holding";
    [HideInInspector]
    public const string TRAMPOLINE = "trampoline";

    // THIS IS A HACK TO SETUP THE BUILDINGPREFABS DICTIONARY
    [System.Serializable]
    public struct BuildPrefab
    {
        public string name;
        public GameObject prefab;
    }
    public BuildPrefab[] BuildPrefabsList;
    public Dictionary<string, GameObject> BuildingPrefabs = new Dictionary<string, GameObject>();

    [Header("SFX")]
    public AudioClip SpawnBuilding;
    public AudioClip DespawnBuilding;
    public AudioClip PickupBuilding;
    public AudioClip PlaceBuilding;
    AudioSource audioSource;

    [Header("Toggle Keys")]
    public KeyCode Toggle1 = KeyCode.A;
    public KeyCode Toggle2 = KeyCode.D;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;

        foreach (BuildPrefab bp in BuildPrefabsList)
        {
            BuildingPrefabs.Add(bp.name, bp.prefab);
        }
    }

    private void Start()
    {
        AudioManager.EnrollSFXSource(GetComponent<AudioSource>());
        audioSource = GetComponent<AudioSource>();
    }

    public void BuildBuilding(string buildingString)
    {
        TileManager.Instance.UnhoverAllTiles();
        // If there is a current building, place it and unhover any tile it is highlighting
        if (CurrBuilding != null)
        {
            // Place current building and set Tile its hovering over as occupied
            CurrBuilding.PlaceBuilding();
            CurrBuilding.TileUnder.OccupyingBuilding = CurrBuilding.gameObject;
            TileManager.Instance.SetTileOccupied(CurrBuilding.TileUnder);
            CurrBuilding = null;
        }

        // Instantiate the new building
        GameObject newBuildingGO = Instantiate(BuildingPrefabs[buildingString], transform.position, this.transform.rotation) as GameObject;
        newBuildingGO.transform.parent = transform;
        Building newBuilding = newBuildingGO.GetComponent<Building>();

        // Get a random unoccupied tile to place it on
        Tile randomTile = TileManager.Instance.GetRandomUnoccupiedTile();

        // Set transform of building to unoccupied tile
        newBuilding.transform.position = randomTile.transform.position + newBuilding.transform.localScale.y / 2 * newBuilding.transform.up;

        // Set building to be hovering over the tile
        newBuilding.TileUnder = randomTile;
        randomTile.SetHoverColor();

        // Set current building
        CurrBuilding = newBuilding;

        // SFX
        audioSource.PlayOneShot(SpawnBuilding);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.Instance.CurrState == GameStateManager.State.Plan)
        {
            RaycastHit hit;
            if (MouseRaycast("Foundation", out hit))
            {
                if (CurrBuilding != null)
                {
                    Tile hitTile = hit.transform.GetComponent<Tile>();
                    if (hitTile.OccupyingBuilding == null)
                    {
                        if (hitTile.Hovered == false)
                        {
                            TileManager.Instance.UnhoverAllTiles();
                            hitTile.SetHoverColor();
                            hitTile.Hovered = true;
                        }

                        CurrBuilding.TileUnder = hitTile;
                        CurrBuilding.transform.position = hit.transform.position + CurrBuilding.transform.localScale.y / 2 * CurrBuilding.transform.up;
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (MouseRaycast("Building", out hit))
                {
                    if (CurrBuilding != null)
                    {
                        if (hit.transform.GetComponent<Building>() == CurrBuilding)
                        {
                            TileManager.Instance.UnhoverAllTiles();

                            // Place current building and set Tile its hovering over as occupied
                            CurrBuilding.TileUnder.OccupyingBuilding = CurrBuilding.gameObject;
                            TileManager.Instance.SetTileOccupied(CurrBuilding.TileUnder);
                            CurrBuilding.PlaceBuilding();

                            // SFX
                            audioSource.PlayOneShot(PlaceBuilding);

                            CurrBuilding = null;
                        }
                        else
                        {
                            TileManager.Instance.UnhoverAllTiles();

                            // Place current building and set Tile its hovering over as occupied
                            CurrBuilding.TileUnder.OccupyingBuilding = CurrBuilding.gameObject;
                            TileManager.Instance.SetTileOccupied(CurrBuilding.TileUnder);
                            CurrBuilding.PlaceBuilding();


                            // Pickup other building we've selected
                            CurrBuilding = hit.transform.GetComponent<Building>();
                            CurrBuilding.TileUnder.OccupyingBuilding = null;
                            CurrBuilding.TileUnder.SetHoverColor();
                            TileManager.Instance.SetTileUnoccupied(CurrBuilding.TileUnder);
                            CurrBuilding.PickUpBuilding();

                            // SFX
                            audioSource.PlayOneShot(PickupBuilding);
                        }
                    }
                    else
                    {
                        // Pickup building
                        CurrBuilding = hit.transform.GetComponent<Building>();
                        CurrBuilding.GetComponent<Building>().TileUnder.OccupyingBuilding = null;
                        TileManager.Instance.SetTileUnoccupied(CurrBuilding.TileUnder);
                        CurrBuilding.GetComponent<Building>().PickUpBuilding();

                        // SFX
                        audioSource.PlayOneShot(PickupBuilding);
                    }
                }
            } else if (Input.GetMouseButtonDown(1))
            {
                CancelBuilding();

                // SFX
                audioSource.PlayOneShot(DespawnBuilding);
            }

            // Handle additional options for specifc buildings bound to the q and e keys
            if (CurrBuilding != null)
            {
                switch (CurrBuilding.GetComponent<Building>().BuildingName)
                {
                    case WALL:
                        // Don't toggle wall height anymore
                        break;
                    case ARROW:
                        if (Input.GetKeyDown(Toggle1))
                        {
                            CurrBuilding.transform.RotateAround(
                                CurrBuilding.transform.position,
                                CurrBuilding.transform.up, 
                                45f);
                        }
                        else if (Input.GetKeyDown(Toggle2))
                        {
                            CurrBuilding.transform.RotateAround(
                                CurrBuilding.transform.position,
                                CurrBuilding.transform.up,
                                -45f);
                        }
                        break;
                    case TRAMPOLINE:
                        // Do nothing
                        break;
                    case HOLDING:
                        Holding currHolding = (Holding)CurrBuilding;
                        if (Input.GetKeyDown(Toggle1))
                        {
                            currHolding.IncrementThreshold();
                        }
                        else if (Input.GetKeyDown(Toggle2))
                        {
                            currHolding.DecrementThreshold();
                        }
                        break;
                }

            }

            if (CurrBuilding != null)
            {
                BuildingSelected = true;
            }
            else
            {
                BuildingSelected = false;
            }
        }
    }

    private bool MouseRaycast(string targetLayerName, out RaycastHit hit)
    {
        int layerMask = 1 << LayerMask.NameToLayer(targetLayerName);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);
    }

    public void CancelBuilding()
    {
        if (CurrBuilding != null)
        {
            MoneyManager.Instance.RefundItem(CurrBuilding.BuildingName);
            TileManager.Instance.UnhoverAllTiles();
            Destroy(CurrBuilding.gameObject);
            CurrBuilding = null;
        }
    }

}
