using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject hitchhikerManager;
    public GameObject GoalEffectPrefab;
    public bool StopSpawning = false;

    public float UpwardOffset;

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(AddPlayers());
        AudioManager.EnrollSFXSource(GetComponent<AudioSource>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator AddPlayers()
    {
        for (int i = 0; i < GoalTrigger.NUM_PACKAGES; i += 1)
        {
            if (StopSpawning)
            {
                break;
            } else
            {
                Instantiate(GoalEffectPrefab, transform.position, Quaternion.Euler(270.0f, 0.0f, 0.0f));
                Instantiate(playerPrefab, transform.position + UpwardOffset * transform.up, transform.localRotation, hitchhikerManager.transform);

                GetComponent<AudioSource>().Play();
            }

            yield return new WaitForSeconds(1f);
        }
    }

}
