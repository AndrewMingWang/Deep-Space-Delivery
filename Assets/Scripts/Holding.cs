using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holding : MonoBehaviour
{
    public int NumHeldThreshhold = 10;
    public List<GameObject> HeldPlayers = new List<GameObject>();

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
            other.gameObject.GetComponent<PlayerMovement>().speed = 0.2f;
            HeldPlayers.Add(other.gameObject);
        }

        if (HeldPlayers.Count > NumHeldThreshhold)
        {
            foreach (GameObject player in HeldPlayers)
            {
                player.GetComponent<PlayerMovement>().speed = 2f;
            }
            HeldPlayers.Clear();
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player"))
        {
            other.gameObject.GetComponent<PlayerMovement>().speed = 2f;
            HeldPlayers.Remove(other.gameObject);
        }
    }
}
