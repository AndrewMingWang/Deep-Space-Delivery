using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingTrigger : MonoBehaviour
{
    public Holding holding;
    public AudioSource Source;

    private void Start()
    {
        AudioManager.EnrollSFXSource(Source);
    }

    private void OnTriggerEnter(Collider other)
    {
        Source.Play();
        holding.TriggerEntered(other);
    }
}
