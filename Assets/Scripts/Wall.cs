using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Building
{
    public bool occupied = false;
    public Tile TileAbove;

    public override void PlaceBuilding()
    {
        base.PlaceBuilding();
        TileAbove.gameObject.SetActive(true);
        TileManager.Instance.AllTiles.Add(TileAbove);
    }

    public override void PickUpBuilding()
    {
        base.PickUpBuilding();

        GameObject buildingAboveGO = TileAbove.OccupyingBuilding;

        if (buildingAboveGO != null)
        {
            Building buildingAbove = buildingAboveGO.GetComponent<Building>();
            buildingAbove.transform.position = TileUnder.transform.position + buildingAbove.transform.localScale.y / 2 * buildingAbove.transform.up;

            buildingAbove.TileUnder = TileUnder;
            TileUnder.OccupyingBuilding = TileAbove.OccupyingBuilding;
            TileManager.Instance.SetTileOccupied(TileUnder);
        }

        TileAbove.OccupyingBuilding = null;
        TileAbove.gameObject.SetActive(false);
        TileManager.Instance.AllTiles.Remove(TileAbove);
    }
}
