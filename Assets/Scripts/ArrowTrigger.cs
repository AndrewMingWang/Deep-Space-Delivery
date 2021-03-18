using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrigger : MonoBehaviour
{
    [Header("Info")]
    public List<int> Seen = new List<int>();

    private void Start()
    {
        AudioManager.EnrollSFXSource(GetComponent<AudioSource>());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            if (!Seen.Contains(other.gameObject.GetInstanceID()))
            {
                Seen.Add(other.gameObject.GetInstanceID());

                // Set unit movement direction
                other.gameObject.GetComponent<UnitMovement>().TargetDirection = transform.parent.forward;

                // Stop and Animate unit
                other.gameObject.GetComponent<UnitMovement>().StopPlayer();
                other.gameObject.GetComponent<UnitMovement>().Animator.SetTrigger("stop");

                // SFX
                GetComponent<AudioSource>().pitch = Random.Range(0.99f, 1.01f);
                GetComponent<AudioSource>().Play();
            }
        }
    }
     
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player"))
        {
            Seen.Remove(other.gameObject.GetInstanceID());
        }
    }
}
