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

    }

    public void BuildWall()
    {
        GameObject newBuilding = Instantiate(Wall, transform.position, Quaternion.identity) as GameObject;
        newBuilding.transform.parent = transform;
        currBuilding = newBuilding.transform;
    }

    public void BuildArrow()
    {
        GameObject newBuilding = Instantiate(Arrow, transform.position, Quaternion.identity) as GameObject;
        newBuilding.transform.parent = transform;
        currBuilding = newBuilding.transform;
    }


    // Update is called once per frame
    void Update()
    {
        RaycastHit[] hits = MouseRaycast();

        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.CompareTag("Foundation") && currBuilding != null)
                {
                    currBuilding.transform.position = hit.point + hit.transform.up * 0.1f;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.transform.CompareTag("Building"))
                    {
                        if (currBuilding != null)
                        {
                            currBuilding.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0.78f, 0.0f, 0.0f, 1.0f));
                            currBuilding = null;
                        }
                        else
                        {
                            currBuilding = hit.transform;
                            currBuilding.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0.78f, 0.0f, 0.0f, 0.5f));
                            
                        }
                    }
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

    private RaycastHit[] MouseRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.RaycastAll(ray);
    }
}