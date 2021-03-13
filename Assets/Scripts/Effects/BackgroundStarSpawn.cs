using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundStarSpawn : MonoBehaviour
{

    public GameObject BackgroundStarEffect;
    public GameObject BackgroundWormholeEffect;
    public GameObject AsteroidPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnBackgroundObjects());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnBackgroundObjects()
    {
        while (true)
        {
            /*float waitTime = Random.Range(5.0f, 10.0f);
            yield return new WaitForSecondsRealtime(waitTime);
            float x = Random.Range(-10.0f, 10.0f);
            float z = Random.Range(-10.0f, 10.0f);
            float choice = Random.Range(0.0f, 1.0f);
            if (choice <= 0.5f)
            {
                Instantiate(BackgroundStarEffect, new Vector3(x, -1.0f, z), Quaternion.Euler(270.0f, 0.0f, 0.0f));
            } else
            {
                GameObject newWormhole = Instantiate(BackgroundWormholeEffect, new Vector3(x, -1.0f, z), Quaternion.Euler(0.0f, 0.0f, 0.0f));
                float scaleX = Random.Range(0.25f, 0.5f);
                float scaleY = Random.Range(0.25f, 0.5f);
                newWormhole.transform.localScale = new Vector2(scaleX, scaleY);
            }*/
            float waitTime = Random.Range(10.0f, 20.0f);
            yield return new WaitForSecondsRealtime(waitTime);
            float x = Random.Range(0.0f, 5.0f);
            GameObject asteroid = Instantiate(AsteroidPrefab, new Vector3(x, -5.0f, 18.0f), Quaternion.identity);
            float size = Random.Range(0.5f, 0.8f);
            asteroid.transform.localScale = new Vector3(size, size, size);
            Destroy(asteroid, 20.0f);
        }
    }

}
