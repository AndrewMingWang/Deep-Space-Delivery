using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : Building
{
    public List<int> seen = new List<int>();

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("player"))
        {
            if (!seen.Contains(other.gameObject.GetInstanceID()))
            {
                seen.Add(other.gameObject.GetInstanceID());
                Vector3 currVelocity = other.gameObject.GetComponent<Rigidbody>().velocity;
                other.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(currVelocity.x, 0, currVelocity.z);
                Vector3 jumpDirection = (260 * Vector3.up);
                other.gameObject.GetComponent<Rigidbody>().AddRelativeForce(jumpDirection);
                other.GetComponent<PlayerMovement>().Animator.SetTrigger("jump");
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
