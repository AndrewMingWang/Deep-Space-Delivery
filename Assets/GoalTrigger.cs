using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{

    public int playersReached = 0;
    public int playersFailed = 0;

    private bool levelOver = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!levelOver && playersReached + playersFailed == 10)
        {
            Debug.Log("RESULTS: " + playersReached + "/10");
            levelOver = true;
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
