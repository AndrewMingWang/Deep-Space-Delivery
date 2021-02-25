using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArrowTrigger : MonoBehaviour
{
    [Header("Info")]
    public List<int> Seen = new List<int>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            if (!Seen.Contains(other.gameObject.GetInstanceID()))
            {
                Seen.Add(other.gameObject.GetInstanceID());

                // Set unit movement direction
                other.gameObject.GetComponent<TutorialUnitMovement>().TargetDirection = transform.parent.forward;

                // Stop and Animate unit
                other.gameObject.GetComponent<TutorialUnitMovement>().StopPlayer();
                other.gameObject.GetComponent<TutorialUnitMovement>().Animator.SetTrigger("stop");

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
