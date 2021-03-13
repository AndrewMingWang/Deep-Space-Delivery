using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{

    private const float BUMP_BACK_FORCE = -4.0f;
    private const float BUMP_UP_FORCE = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            Vector3 dir = collision.gameObject.GetComponent<UnitMovement>().TargetDirection;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(BUMP_BACK_FORCE * dir + BUMP_UP_FORCE * collision.transform.up, ForceMode.VelocityChange);
            collision.gameObject.GetComponent<UnitMovement>().Animator.SetTrigger("bump");
        }
    }

}
