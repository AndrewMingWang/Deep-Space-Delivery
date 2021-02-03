using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{

    public static int NUM_PLAYERS = 20;

    public int playersReached = 0;
    public int playersFailed = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playersReached + playersFailed == NUM_PLAYERS)
        {
            Debug.Log("HITCHHIKERS REACHED GOAL: " + (playersReached * 100.0f / NUM_PLAYERS) + "%");
            playersReached = 0;
            playersFailed = 0;
            Debug.Log("MONEY LEFT: $" + GameObject.FindGameObjectWithTag("moneyManager").GetComponent<MoneyManager>().GetRemainingMoney());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            other.gameObject.SetActive(false);
            playersReached += 1;
        }
    }

}
