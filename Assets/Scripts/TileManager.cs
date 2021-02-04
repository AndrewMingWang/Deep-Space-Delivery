using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;

    public Tile[] AllTiles;

    private void Awake()
    {
        AllTiles = transform.GetComponentsInChildren<Tile>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    public void UnhoverAllTiles()
    {
        foreach (Tile tile in AllTiles)
        {
            tile.SetBaseColor();
            tile.Hovered = false;
        }
    }
}
