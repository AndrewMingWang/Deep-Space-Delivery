using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileRandomizer : MonoBehaviour
{
    [System.Serializable]
    public struct TilePrefab
    {
        public GameObject prefab;
        public float weight;
    }

    public List<TilePrefab> TilePrefabs = new List<TilePrefab>();

    [ContextMenu("Randomize")]
    private void RandomizeTiles()
    {
        int numTiles = transform.childCount;
        Debug.Log("Randomizing " + numTiles.ToString() + " Tiles");
        List<Vector3> positions = new List<Vector3>();
        List<Quaternion> rotations = new List<Quaternion>();
        List<GameObject> occupiers = new List<GameObject>();
        List<bool> tileManagerOnAwakes = new List<bool>();
        List<GameObject> toBeDestroyed = new List<GameObject>();


        // Record existing info
        for (int i = 0; i < numTiles; i++)
        {
            positions.Add(transform.GetChild(i).transform.position);
            rotations.Add(transform.GetChild(i).transform.rotation);
            occupiers.Add(transform.GetChild(i).GetComponent<Tile>().OccupyingBuilding);
            tileManagerOnAwakes.Add(transform.GetChild(i).GetComponent<Tile>().AddToTileManagerOnAwake);
            toBeDestroyed.Add(transform.GetChild(i).gameObject);
        }

        // Destroy current tiles
        foreach (GameObject t in toBeDestroyed)
        {
            DestroyImmediate(t);
        }

        // Regenerate tiles
        float totalWeight = 0;
        List<float> cutoffs = new List<float>();
        foreach (TilePrefab t in TilePrefabs)
        {
            totalWeight += t.weight;
            cutoffs.Add(totalWeight);
        }
        // Debug.Log(cutoffs);

        for (int i = 0; i < numTiles; i++)
        {
            float r = Random.Range(0, totalWeight);
            // Debug.Log(r);
            for (int j = 0; j < cutoffs.Count; j++)
            {
                if (r <= cutoffs[j])
                {
                    GameObject newTile = Instantiate(TilePrefabs[j].prefab, positions[i], rotations[i], transform);
                    newTile.GetComponent<Tile>().OccupyingBuilding = occupiers[i];
                    newTile.GetComponent<Tile>().AddToTileManagerOnAwake = tileManagerOnAwakes[i];
                    break;
                }
            }
        }
    }


}
