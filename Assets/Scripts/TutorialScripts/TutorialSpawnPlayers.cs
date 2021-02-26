using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSpawnPlayers : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject hitchhikerManager;

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
        for (int i = 0; i < TutorialGoalTrigger.NUM_PLAYERS; i += 1)
        {
            Instantiate(playerPrefab, transform.position, transform.localRotation, hitchhikerManager.transform);
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(1f);
        }
    }

}
