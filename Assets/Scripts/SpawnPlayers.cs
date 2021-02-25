using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject hitchhikerManager;

    public float UpwardOffset;

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(AddPlayers());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator AddPlayers()
    {
        for (int i = 0; i < GoalTrigger.NUM_PLAYERS; i += 1)
        {
            Instantiate(playerPrefab, transform.position + UpwardOffset * transform.up, transform.localRotation, hitchhikerManager.transform);

            yield return new WaitForSeconds(1f);
        }
    }

}
