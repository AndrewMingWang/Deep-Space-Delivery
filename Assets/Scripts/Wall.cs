using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Building
{
    public bool occupied = false;
    public Tile TileAbove;

    public List<int> seen = new List<int>();

    

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            if (!seen.Contains(other.gameObject.GetInstanceID()))
            {
                seen.Add(other.gameObject.GetInstanceID());
                Vector3 dir = other.gameObject.GetComponent<PlayerMovement>().direction;

                other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                other.gameObject.GetComponent<Rigidbody>().AddForce(-4f*dir + other.transform.up, ForceMode.VelocityChange);
                other.gameObject.GetComponent<PlayerMovement>().Animator.SetTrigger("bump");
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            seen.Remove(other.gameObject.GetInstanceID());
        }
    }

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
