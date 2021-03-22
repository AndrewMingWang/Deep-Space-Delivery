using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineTrigger : MonoBehaviour
{
    [Header("Mechanics")]
    public float JumpForce;

    [Header("Info")]
    public List<int> Seen = new List<int>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            if (!Seen.Contains(other.gameObject.GetInstanceID()))
            {
                Seen.Add(other.gameObject.GetInstanceID());

                // Reset vertical velocity to zero
                Vector3 currVelocity = other.gameObject.GetComponent<Rigidbody>().velocity;
                other.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(currVelocity.x, 0, currVelocity.z);

                // Apply jump vertical force
                Vector3 jumpDirection = (JumpForce * Vector3.up);
                other.gameObject.GetComponent<Rigidbody>().AddRelativeForce(jumpDirection);

                // Animate
                other.GetComponent<UnitMovement>().Animator.SetTrigger("jump");
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
