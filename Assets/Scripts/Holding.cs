using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holding : Building
{
    public float scaleThreshold = 0.11f;
    public List<GameObject> HeldPlayers = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            other.gameObject.GetComponent<PlayerMovement>().speed = 0.2f;
            HeldPlayers.Add(other.gameObject);

            Vector3 currScale = transform.localScale;
            transform.localScale = Vector3.Max(currScale - new Vector3(0, 0.05f, 0), Vector3.zero);
            Vector3 currPosition = transform.localPosition;
            transform.localPosition = new Vector3(currPosition.x, currPosition.y - currScale.y / 2 + transform.localScale.y / 2, currPosition.z);
        }

        if (transform.localScale.y < scaleThreshold)
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

            Vector3 currScale = transform.localScale;
            transform.localScale = currScale + new Vector3(0, 0.05f, 0);
            Vector3 currPosition = transform.localPosition;
            transform.localPosition = new Vector3(currPosition.x, currPosition.y - currScale.y / 2 + transform.localScale.y / 2, currPosition.z);
        }
    }
}
