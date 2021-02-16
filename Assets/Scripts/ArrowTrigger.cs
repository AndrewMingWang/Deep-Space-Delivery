using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrigger : MonoBehaviour
{
    public List<int> seen = new List<int>();

    public Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        //
    }

    // Update is called once per frame
    void Update()
    {
        direction = transform.parent.transform.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            if (!seen.Contains(other.gameObject.GetInstanceID()))
            {
                seen.Add(other.gameObject.GetInstanceID());
                Debug.Log(direction);
                other.gameObject.GetComponent<PlayerMovement>().direction = direction;
                other.gameObject.GetComponent<PlayerMovement>().StopPlayer();
                other.gameObject.GetComponent<PlayerMovement>().Animator.SetTrigger("stop");

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player"))
        {
            seen.Remove(other.gameObject.GetInstanceID());
        }
    }
}
