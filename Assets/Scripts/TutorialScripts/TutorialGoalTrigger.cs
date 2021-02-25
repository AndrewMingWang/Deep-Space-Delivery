using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialGoalTrigger : MonoBehaviour
{

    public static readonly int NUM_PLAYERS = 1;

    public int PlayersReached = 0;
    public int PlayersFailed = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            AudioManager.Play(AudioManager.SFX_REACH_GOAL);
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
