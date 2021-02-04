using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    public static bool isBuilding = false;

    public Transform currBuilding;

    // "wall" - Wall prefab
    // "arrow" - Arrow prefab
    [HideInInspector]
    public const string WALL = "wall";
    [HideInInspector]
    public const string ARROW = "arrow";

    // THIS IS A HACK TO SETUP THE BUILDINGPREFABS DICTIONARY
    [System.Serializable]
    public struct BuildPrefab
    {
        public string name;
        public GameObject prefab;
    }
    public BuildPrefab[] BuildPrefabsList;
    public Dictionary<string, GameObject> BuildingPrefabs = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;

        foreach (BuildPrefab bp in BuildPrefabsList)
        {
            BuildingPrefabs.Add(bp.name, bp.prefab);
        }
    }

    public void BuildBuilding(string buildingString)
    {
        // If there is a current building, place it and unhover any tile it is highlighting
        if (currBuilding != null)
        {
            TileManager.Instance.UnhoverAllTiles();
            currBuilding.GetComponent<Building>().setColorPlaced();
            currBuilding = null;
        }

        // Instantiate the new building
        GameObject newBuilding = Instantiate(BuildingPrefabs[buildingString], transform.position, Quaternion.identity) as GameObject;
        newBuilding.transform.parent = transform;

        // Get a random unoccupied tile to place it on
        Tile randomTile = TileManager.Instance.RandomUnoccupiedTile();

        // Set transform of building to unoccupied tile
        newBuilding.transform.position = randomTile.transform.position + newBuilding.transform.localScale.y / 2 * newBuilding.transform.up;

        // Set tile and building to occupied
        newBuilding.GetComponent<Building>().HoveringTile = randomTile;

        // Set current building
        currBuilding = newBuilding.transform;

        // Update money for the tile
        MoneyManager.Instance.PurchaseItem(buildingString);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (MouseRaycast("Foundation", out hit))
        {
            if (currBuilding != null)
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

                    currBuilding.GetComponent<Building>().HoveringTile = hitTile;
                    currBuilding.position = hit.transform.position + currBuilding.localScale.y / 2 * currBuilding.up;
                }  
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (MouseRaycast("Building", out hit))
            {
                if (currBuilding != null)
                {
                    if (hit.transform == currBuilding)
                    {
                        TileManager.Instance.UnhoverAllTiles();

                        // Place current building and set Tile its hovering over as occupied
                        currBuilding.GetComponent<Building>().setColorPlaced();
                        currBuilding.GetComponent<Building>().HoveringTile.OccupyingBuilding = currBuilding.gameObject;
                        TileManager.Instance.OccupiedTiles.Add(currBuilding.GetComponent<Building>().HoveringTile);
                        TileManager.Instance.UnoccupiedTiles.Remove(currBuilding.GetComponent<Building>().HoveringTile);

                        currBuilding = null;
                    }
                    else
                    {
                        TileManager.Instance.UnhoverAllTiles();

                        // Place current building and set Tile its hovering over as occupied
                        currBuilding.GetComponent<Building>().setColorPlaced();
                        currBuilding.GetComponent<Building>().HoveringTile.OccupyingBuilding = currBuilding.gameObject;
                        TileManager.Instance.OccupiedTiles.Add(currBuilding.GetComponent<Building>().HoveringTile);
                        TileManager.Instance.UnoccupiedTiles.Remove(currBuilding.GetComponent<Building>().HoveringTile);

                        // Pickup other building we've selected
                        currBuilding = hit.transform;
                        currBuilding.GetComponent<Building>().HoveringTile.OccupyingBuilding = null;
                        currBuilding.GetComponent<Building>().HoveringTile.SetHoverColor();
                        currBuilding.GetComponent<Building>().setColorSelected();
                        TileManager.Instance.UnoccupiedTiles.Add(currBuilding.GetComponent<Building>().HoveringTile);
                        TileManager.Instance.OccupiedTiles.Remove(currBuilding.GetComponent<Building>().HoveringTile);
                    }
                }
                else
                {
                    // Pickup building
                    currBuilding = hit.transform;
                    currBuilding.GetComponent<Building>().HoveringTile.OccupyingBuilding = null;
                    currBuilding.GetComponent<Building>().setColorSelected();
                    TileManager.Instance.UnoccupiedTiles.Add(currBuilding.GetComponent<Building>().HoveringTile);
                    TileManager.Instance.OccupiedTiles.Remove(currBuilding.GetComponent<Building>().HoveringTile);
                }
            }
        }

        // Handle additional options for specifc buildings bound to the q and e keys
        if (currBuilding != null)
        {
            switch (currBuilding.GetComponent<Building>().BuildingName)
            {
                case WALL:
                    if (Input.GetKey(KeyCode.E))
                    {
                        Vector3 currScale = currBuilding.transform.localScale;
                        Vector3 currScaleMax = new Vector3(currScale.x, 5f, currScale.z);
                        currBuilding.transform.localScale = Vector3.Min(currScale + new Vector3(0, 0.8f * Time.deltaTime, 0), currScaleMax);
                        
                    }
                    else if (Input.GetKey(KeyCode.Q))
                    {
                        Vector3 currScale = currBuilding.transform.localScale;
                        Vector3 currScaleMin = new Vector3(currScale.x, 0.1f, currScale.z);
                        currBuilding.transform.localScale = Vector3.Max(currScale - new Vector3(0, 0.8f * Time.deltaTime, 0), currScaleMin);
                    }
                    break;
                case ARROW:
                    if (Input.GetKey(KeyCode.E))
                    {
                        currBuilding.RotateAround(transform.position, transform.up, Time.deltaTime * 90f);
                    }
                    else if (Input.GetKey(KeyCode.Q))
                    {
                        currBuilding.RotateAround(transform.position, transform.up, -Time.deltaTime * 90f);
                    }
                    break;
            }

        }

        if (currBuilding != null){
            isBuilding = true;
        } else {
            isBuilding = false;
        }
    }

    private bool MouseRaycast(string targetLayerName, out RaycastHit hit)
    {
        int layerMask = 1 << LayerMask.NameToLayer(targetLayerName);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);
    }
}
