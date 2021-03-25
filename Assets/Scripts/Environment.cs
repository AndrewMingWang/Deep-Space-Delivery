using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{

    private const float BUMP_BACK_FORCE = -4.0f;
    private const float BUMP_UP_FORCE = 1.0f;

    private void Start()
    {
        AudioManager.EnrollSFXSource(GetComponent<AudioSource>());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            Vector3 dir = collision.gameObject.GetComponent<Dog>().TargetDirection;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(BUMP_BACK_FORCE * dir + BUMP_UP_FORCE * collision.transform.up, ForceMode.VelocityChange);
            collision.gameObject.GetComponent<Dog>().Animator.SetTrigger("bump");

            // Sound Effect
            GetComponent<AudioSource>().Play();
        }
    }

}
