using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
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
            Vector3 jumpDirection = (260*Vector3.up);
            Debug.Log(jumpDirection);
            other.gameObject.GetComponent<Rigidbody>().AddRelativeForce(jumpDirection);
        }
    }
}
