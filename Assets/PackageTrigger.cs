using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            UnitMovement player = other.gameObject.GetComponent<UnitMovement>();
            if (!player.PackageShown)
            {
                other.gameObject.GetComponent<UnitMovement>().ShowPackage();
                transform.parent.gameObject.SetActive(false);
            }
        }
    }
}
