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
    public static string WALL = "wall";
    [HideInInspector]
    public static string ARROW = "arrow";

    // THIS IS A HACK TO SETUP THE BuildingPrefabs DICTIONARY
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
            currBuilding.GetComponent<Building>().setColorPlaced();
            currBuilding = null;
        }

        GameObject newBuilding = Instantiate(BuildingPrefabs[buildingString], transform.position, Quaternion.identity) as GameObject;

        newBuilding.transform.parent = transform;
        newBuilding.transform.position = transform.position + newBuilding.transform.localScale.y / 2 * newBuilding.transform.up;

        currBuilding = newBuilding.transform;
        MoneyManager.instance.PurchaseItem(buildingString);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (MouseRaycast("Foundation", out hit))
        {
            if (currBuilding != null)
            {
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


        if (Input.GetKey(KeyCode.E))
        {
            if (currBuilding != null)
            {
                currBuilding.RotateAround(transform.position, transform.up, Time.deltaTime * 90f);
            }
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            if (currBuilding != null)
            {
                currBuilding.RotateAround(transform.position, transform.up, -Time.deltaTime * 90f);
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
