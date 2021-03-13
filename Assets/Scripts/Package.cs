using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package : MonoBehaviour
{
    public bool Grounded = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!Grounded)
        {
            if (collision.transform.CompareTag("Foundation"))
            {
                GetComponent<Rigidbody>().useGravity = true;
                Grounded = true;
                PackagesSpawner.Instance.NumLanded += 1;
            }
            else if (collision.transform.CompareTag("Package"))
            {
                if (collision.transform.GetComponent<Package>().Grounded)
                {
                    GetComponent<Rigidbody>().useGravity = true;
                    Grounded = true;
                    PackagesSpawner.Instance.NumLanded += 1;
                }
            }
        }
    }
}
