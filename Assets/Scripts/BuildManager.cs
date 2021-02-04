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
        if (currBuilding != null)
        {
            TileManager.Instance.UnhoverAllTiles();
            currBuilding.GetComponent<Building>().setColorPlaced();
            currBuilding = null;
        }

        GameObject newBuilding = Instantiate(BuildingPrefabs[buildingString], transform.position, Quaternion.identity) as GameObject;

        newBuilding.transform.parent = transform;
        newBuilding.transform.position = transform.position + newBuilding.transform.localScale.y / 2 * newBuilding.transform.up;

        currBuilding = newBuilding.transform;
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
                if (hitTile.Hovered == false)
                {
                    TileManager.Instance.UnhoverAllTiles();
                    hitTile.SetHoverColor();
                    hitTile.Hovered = true;
                }
                currBuilding.position = hit.point + currBuilding.localScale.y / 2 * currBuilding.up;
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
                        currBuilding.GetComponent<Building>().setColorPlaced();
                        currBuilding = null;
                    }
                    else
                    {
                        currBuilding.GetComponent<Building>().setColorPlaced();
                        currBuilding = hit.transform;
                        currBuilding.GetComponent<Building>().setColorSelected();
                    }
                }
                else
                {
                    currBuilding = hit.transform;
                    currBuilding.GetComponent<Building>().setColorSelected();
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
