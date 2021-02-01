using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    public static bool isBuilding = false;

    public Transform currBuilding;

    public GameObject Wall;
    public GameObject Arrow;

    private MoneyManager moneyManager;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
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
                currBuilding.position = hit.point + currBuilding.localScale.y / 2 * currBuilding.up;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (MouseRaycast("Building", out hit)) {
                if (hit.transform == currBuilding)
                {
                    currBuilding.GetComponent<Building>().setColorPlaced();
                    currBuilding = null;
                } else
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
        } else if (Input.GetKey(KeyCode.Q))
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