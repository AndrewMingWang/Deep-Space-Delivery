using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : Building
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("player"))
        {
            Vector3 currVelocity = other.gameObject.GetComponent<Rigidbody>().velocity;
            other.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(currVelocity.x, 0, currVelocity.z);
            Vector3 jumpDirection = (260*Vector3.up);
            other.gameObject.GetComponent<Rigidbody>().AddRelativeForce(jumpDirection);
            other.GetComponent<PlayerMovement>().Animator.SetTrigger("jump");

        }
    }
}
