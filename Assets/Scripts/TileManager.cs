using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;
    public GameObject YTicks;

    public Tile[] EnvironmentTiles;
    public List<Tile> AllTiles;
    public List<Tile> UnoccupiedTiles;
    public List<Tile> OccupiedTiles;
    private bool _sidesOn;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        EnvironmentTiles = transform.GetComponentsInChildren<Tile>();
        foreach (Tile tile in EnvironmentTiles)
        {
            if (tile.AddToTileManagerOnAwake){
                if (tile.OccupyingBuilding == null && !tile.EnvOccupied)
                {
                    UnoccupiedTiles.Add(tile);
                }
                if (tile.OccupyingBuilding != null || tile.EnvOccupied)
                {
                    OccupiedTiles.Add(tile);
                }
                AllTiles.Add(tile);
            }
            
        }
        _sidesOn = false;
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

    public void AddUnoccupiedTile(Tile tile){
        UnoccupiedTiles.Add(tile);
        AllTiles.Add(tile);
    }
    
    // NEVER USE THIS unless you know what you're doing
    public void RemoveTile(Tile tile){
        AllTiles.Remove(tile);
        OccupiedTiles.Remove(tile);
        UnoccupiedTiles.Remove(tile);
    }

    public void EnableTileSides(){
        _sidesOn = !_sidesOn;
        foreach (Tile tile in AllTiles){
            tile.EnableSides(_sidesOn);
        }
        if (YTicks != null){
            YTicks.SetActive(_sidesOn);
        }
    }
}
