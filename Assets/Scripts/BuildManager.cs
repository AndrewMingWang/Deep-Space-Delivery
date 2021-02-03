using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    public static bool isBuilding = false;

    public Transform currBuilding;

    // 0 - Wall
    // 1 - Arrow
    public GameObject[] BuildingPrefabs;

    private GameObject Wall;
    private GameObject Arrow;
    private MoneyManager moneyManager;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;

        Wall = BuildingPrefabs[0];
        Arrow = BuildingPrefabs[1];
        moneyManager = GameObject.FindGameObjectWithTag("moneyManager").GetComponent<MoneyManager>();
    }

    public void BuildWall()
    {
        GameObject newBuilding = Instantiate(Wall, transform.position, Quaternion.identity) as GameObject;

        newBuilding.transform.parent = transform;
        newBuilding.transform.position = transform.position + newBuilding.transform.localScale.y / 2 * newBuilding.transform.up;

        currBuilding = newBuilding.transform;
        moneyManager.PurchaseItem(MoneyManager.ITEM_WALL);
    }

    public void BuildArrow()
    {
        GameObject newBuilding = Instantiate(Arrow, transform.position, Quaternion.identity) as GameObject;

        newBuilding.transform.parent = transform;
        newBuilding.transform.position = transform.position + newBuilding.transform.localScale.y / 2 * newBuilding.transform.up;

        currBuilding = newBuilding.transform;
        moneyManager.PurchaseItem(MoneyManager.ITEM_ARROW);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (MouseRaycast("Foundation", out hit))
        {
            if (currBuilding != null)
            {
                Debug.Log(hit.point + currBuilding.localScale.y / 2 * currBuilding.up);
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
    }

    private bool MouseRaycast(string targetLayerName, out RaycastHit hit)
    {
        int layerMask = 1 << LayerMask.NameToLayer(targetLayerName);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);
    }
}
