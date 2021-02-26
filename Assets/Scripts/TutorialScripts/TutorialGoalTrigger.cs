using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialGoalTrigger : MonoBehaviour
{

    public static int NUM_PLAYERS = 1;

    public int PlayersReached = 0;
    public int PlayersFailed = 0;

    // Start is called before the first frame update
    void Start()
    {
        NUM_PLAYERS = 1;
        AudioManager.EnrollSFXSource(GetComponent<AudioSource>());
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void updateGoalQuota(int val){
        NUM_PLAYERS = val;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            GetComponent<AudioSource>().Play();
            PlayersReached += 1;
            other.gameObject.SetActive(false);
        }
    }

    public void ResetPlayerResults()
    {
        PlayersReached = 0;
        PlayersFailed = 0;
    }

    public bool IsLevelFailed()
    {
        return PlayersFailed == NUM_PLAYERS;
    }

    public bool IsLevelPassed()
    {
        return PlayersReached == NUM_PLAYERS;
    }
}
