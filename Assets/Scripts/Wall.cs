﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Building
{
    [Header("Mechanics")]
    public float BumpBackForce;
    public float BumpUpForce;

    [Header("Info")]
    public bool Occupied = false;
    public Tile TileAbove;
    public List<int> Seen = new List<int>();

    [Header("Highlights")]
    public GameObject Highlights;

    private void Start()
    {
        AudioManager.EnrollSFXSource(GetComponent<AudioSource>());
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            if (!Seen.Contains(other.gameObject.GetInstanceID()))
            {
                Seen.Add(other.gameObject.GetInstanceID());

                // Bump unit backwards and up
                Vector3 dir = other.gameObject.GetComponent<Dog>().TargetDirection;
                other.gameObject.GetComponent<Rigidbody>().AddForce(-BumpBackForce * dir + BumpUpForce * other.transform.up, ForceMode.VelocityChange);

                // Animate
                other.gameObject.GetComponent<Dog>().Animator.SetTrigger("bump");

                // Sound Effect
                GetComponent<AudioSource>().Play();
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            Seen.Remove(other.gameObject.GetInstanceID());
        }
    }

    public override void PlaceBuilding()
    {
        base.PlaceBuilding();
        TileAbove.gameObject.SetActive(true);
        TileManager.Instance.AddUnoccupiedTile(TileAbove);
    }

    public override void PickUpBuilding()
    {
        base.PickUpBuilding();

        GameObject buildingAboveGO = TileAbove.OccupyingBuilding;

        if (buildingAboveGO != null)
        {
            Building buildingAbove = buildingAboveGO.GetComponent<Building>();
            buildingAbove.transform.position = TileUnder.transform.position + buildingAbove.SpawnHeight * buildingAbove.transform.up;

            buildingAboveGO.transform.parent = TileUnder.transform;
            buildingAbove.TileUnder = TileUnder;
            TileUnder.OccupyingBuilding = TileAbove.OccupyingBuilding;
            TileManager.Instance.SetTileOccupied(TileUnder);

            RepositionBuildingsAbove(buildingAboveGO);
        }

        TileAbove.OccupyingBuilding = null;
        TileAbove.gameObject.SetActive(false);
        TileManager.Instance.RemoveTile(TileAbove);
    }

    public void RepositionBuildingsAbove(GameObject buildingGO)
    {
        Wall wall = buildingGO.GetComponent<Wall>();
        if (wall != null)
        {
            GameObject buildingAboveGO = wall.TileAbove.OccupyingBuilding;
            if (buildingAboveGO != null)
            {
                buildingAboveGO.transform.parent = wall.TileAbove.transform;
                Building buildingAbove = buildingAboveGO.GetComponent<Building>();
                buildingAbove.transform.position = wall.TileAbove.transform.position + buildingAbove.SpawnHeight * buildingAbove.transform.up;

                RepositionBuildingsAbove(buildingAboveGO);
            }
        }
    }

    public override void setColorPickedUp()
    {
        meshRenderer.material.SetColor("_Color", SelectedColor);
        Highlights.SetActive(false);
    }

    public override void setColorPlaced()
    {
        meshRenderer.material.SetColor("_Color", PlacedColor);
        Highlights.SetActive(true);
    }

    public override void HardReset()
    {
        TileManager.Instance.RemoveTile(TileAbove);
    }
}
