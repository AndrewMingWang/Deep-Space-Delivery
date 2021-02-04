using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrigger : MonoBehaviour
{

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
            other.gameObject.GetComponent<PlayerMovement>().direction = direction;
            other.gameObject.transform.rotation = Quaternion.LookRotation(direction);
        
        }
    }

}
