using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{

    public GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AddPlayers());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator AddPlayers()
    {
        for (int i = 0; i < 10; i += 1)
        {
            Instantiate(playerPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
    }

}
