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
        List<bool> envOccupied = new List<bool>();
        List<bool> unbuildableShading = new List<bool>();
        List<bool> windTiles = new List<bool>();
        List<bool> tileManagerOnAwakes = new List<bool>();
        List<GameObject> toBeDestroyed = new List<GameObject>();

        List<bool> showleft = new List<bool>();
        List<bool> showtop = new List<bool>();
        List<bool> showright = new List<bool>();
        List<bool> showbottom = new List<bool>();
        List<Material> sidesmaterial = new List<Material>();


        // Record existing info
        for (int i = 0; i < numTiles; i++)
        {
            positions.Add(transform.GetChild(i).transform.position);
            rotations.Add(transform.GetChild(i).transform.rotation);
            occupiers.Add(transform.GetChild(i).GetComponent<Tile>().OccupyingBuilding);
            envOccupied.Add(transform.GetChild(i).GetComponent<Tile>().EnvOccupied);
            unbuildableShading.Add(transform.GetChild(i).GetComponent<Tile>().UnbuildableShadingOn);
            windTiles.Add(transform.GetChild(i).GetComponent<Tile>().windTile);
            tileManagerOnAwakes.Add(transform.GetChild(i).GetComponent<Tile>().AddToTileManagerOnAwake);
            toBeDestroyed.Add(transform.GetChild(i).gameObject);
            showleft.Add(transform.GetChild(i).GetComponent<Tile>().showLeft);
            showtop.Add(transform.GetChild(i).GetComponent<Tile>().showTop);
            showright.Add(transform.GetChild(i).GetComponent<Tile>().showRight);
            showbottom.Add(transform.GetChild(i).GetComponent<Tile>().showBottom);
            sidesmaterial.Add(transform.GetChild(i).GetComponent<Tile>().sidesMaterial);

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
                    newTile.GetComponent<Tile>().EnvOccupied = envOccupied[i];
                    newTile.GetComponent<Tile>().OccupyingBuilding = occupiers[i];
                    newTile.GetComponent<Tile>().AddToTileManagerOnAwake = tileManagerOnAwakes[i];
                    newTile.GetComponent<Tile>().windTile = windTiles[i];
                    newTile.GetComponent<Tile>().UnbuildableShadingOn = unbuildableShading[i];
                    newTile.GetComponent<Tile>().showLeft = showleft[i];
                    newTile.GetComponent<Tile>().showTop = showtop[i];
                    newTile.GetComponent<Tile>().showRight = showright[i];
                    newTile.GetComponent<Tile>().showBottom = showbottom[i];
                    newTile.GetComponent<Tile>().sidesMaterial = sidesmaterial[i];
                    break;
                }
            }
        }
    }


}
