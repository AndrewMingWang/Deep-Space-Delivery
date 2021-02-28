using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTileFloatAnimation : MonoBehaviour
{
    public Vector2 MaxYRange;
    [HideInInspector]
    public float MaxY;
    public float Period;

    private float t;
    const float TWOPI = 2 * Mathf.PI;

    private void Start()
    {
        MaxY = Random.Range(MaxYRange.x, MaxYRange.y);
        t = Random.Range(0, TWOPI);
        transform.position += Vector3.up * MaxY * Mathf.Sin(t * TWOPI / Period);
    }

    void Update()
    {
        t += Time.deltaTime;

        Vector3 offset = Vector3.up * MaxY * Mathf.Sin(t * TWOPI / Period) * Time.deltaTime;

        transform.position += offset;

        Tile tile = GetComponent<Tile>();
        if (tile != null)
        {
            if (tile.OccupyingBuilding != null)
            {
                tile.OccupyingBuilding.transform.position += offset;
            }
        }
    }
}
