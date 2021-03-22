using System.Collections;
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

    private KeyCode Toggle1 = KeyCode.E;
    private KeyCode Toggle2 = KeyCode.Q;

    [Header("Permissions")]
    public bool allowBuildingNewBuildings = true;
    public bool allowPickingUpBuildings = true;
    public bool allowRotatingBuildings = true;
    public bool allowDeletingBuildings = true;

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
        if (allowBuildingNewBuildings){
            // If there is a current building, cancel it
            CancelBuilding();

            // Instantiate the new building
            GameObject newBuildingGO = Instantiate(BuildingPrefabs[buildingString], transform.position, this.transform.rotation) as GameObject;
            newBuildingGO.transform.parent = transform;
            Building newBuilding = newBuildingGO.GetComponent<Building>();

            // Get a random unoccupied tile to place it on
            Tile randomTile = TileManager.Instance.GetRandomUnoccupiedTile();

            // Set transform of building to unoccupied tile
            newBuilding.transform.position = randomTile.transform.position + newBuilding.SpawnHeight * newBuilding.transform.up;

            // Set building to be hovering over the tile
            newBuilding.TileUnder = randomTile;
            randomTile.SetHoverColor();

            // Set current building
            CurrBuilding = newBuilding;

            // SFX
            audioSource.PlayOneShot(SpawnBuilding);
        }
        
        
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
                    if (hitTile != null){
                        if (hitTile.OccupyingBuilding == null && !hitTile.EnvOccupied)
                        {
                            if (hitTile.Hovered == false)
                            {
                                TileManager.Instance.UnhoverAllTiles();
                                hitTile.SetHoverColor();
                                hitTile.Hovered = true;
                            }

                        CurrBuilding.TileUnder = hitTile;
                        CurrBuilding.transform.position = hit.transform.position + CurrBuilding.GetComponent<Building>().SpawnHeight * CurrBuilding.transform.up;
                        }
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
                            if (Input.GetKey(KeyCode.LeftShift))
                            {
                                MoneyManager.Instance.CopyItem();
                            }
                        }
                        else
                        {
                            /*
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
                            */
                        }
                    }
                    else if (allowPickingUpBuildings)
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
            } else if (Input.GetMouseButtonDown(1) && allowDeletingBuildings)
            {
                CancelBuilding();               
            }

            // Handle additional options for specifc buildings bound to the a and d keys
            if (CurrBuilding != null && allowRotatingBuildings)
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
                            CurrBuilding.GetComponent<AudioSource>().Play();
                        }
                        else if (Input.GetKeyDown(Toggle2))
                        {
                            CurrBuilding.transform.RotateAround(
                                CurrBuilding.transform.position,
                                CurrBuilding.transform.up,
                                -45f);
                            CurrBuilding.GetComponent<AudioSource>().Play();
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

            audioSource.PlayOneShot(DespawnBuilding);
        }
    }

}
