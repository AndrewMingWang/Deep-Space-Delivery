using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;

    public Tile[] AllTiles;
    public List<Tile> UnoccupiedTiles;
    public List<Tile> OccupiedTiles;

    private void Awake()
    {
        AllTiles = transform.GetComponentsInChildren<Tile>();
        foreach (Tile tile in AllTiles)
        {
            if (tile.OccupyingBuilding == null)
            {
                UnoccupiedTiles.Add(tile);
            }
            if (tile.OccupyingBuilding != null)
            {
                OccupiedTiles.Add(tile);
            }
        }
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

    public void SetTileOccupied(Tile tile)
    {
        OccupiedTiles.Add(tile);
        UnoccupiedTiles.Remove(tile);
    }

    public void SetTileUnoccupied(Tile tile)
    {
        OccupiedTiles.Remove(tile);
        UnoccupiedTiles.Add(tile);
    }

    public Tile GetRandomUnoccupiedTile()
    {
        int n = UnoccupiedTiles.Count;
        int i = Random.Range(0, n);
        return UnoccupiedTiles[i];
    }
}
