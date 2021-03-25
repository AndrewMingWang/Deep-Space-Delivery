using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingTrigger : MonoBehaviour
{
    public Holding holding;

    private void OnTriggerEnter(Collider other)
    {
        holding.TriggerEntered(other);
    }
}
