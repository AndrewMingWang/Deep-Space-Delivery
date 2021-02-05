using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : Building
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("player"))
        {
            Vector3 currVelocity = other.gameObject.GetComponent<Rigidbody>().velocity;
            other.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(currVelocity.x, 0, currVelocity.z);
            Vector3 jumpDirection = (260*Vector3.up);
            other.gameObject.GetComponent<Rigidbody>().AddRelativeForce(jumpDirection);
        }
    }
}
