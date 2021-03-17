using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageTrigger : MonoBehaviour
{
    bool played = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!played)
        {
            GetComponentInParent<Package>().PlayLanding();
            played = true;
        }
    }
}
