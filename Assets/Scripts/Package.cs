using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package : MonoBehaviour
{
    public AudioSource FlyingSource;
    public AudioSource LandingSource;

    public bool Grounded = false;

    private void Awake()
    {
        AudioManager.EnrollSFXSource(FlyingSource);
        AudioManager.EnrollSFXSource(LandingSource);
        FlyingSource.Play();
        StartCoroutine(ForceLand());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!Grounded)
        {
            if (collision.transform.CompareTag("Foundation"))
            {
                Land();
            }
            else if (collision.transform.CompareTag("Package"))
            {
                if (collision.transform.GetComponent<Package>().Grounded)
                {
                    Land();
                }
            }
        }
    }

    private IEnumerator ForceLand()
    {
        yield return new WaitForSeconds(5.0f);
        if (!Grounded)
        {
            Land();
        }
    }

    private void Land()
    {
        GetComponent<Rigidbody>().useGravity = true;
        Grounded = true;
        PackagesSpawner.Instance.NumLanded += 1;
    }

    public void PlayLanding()
    {
        LandingSource.Play();
    }
}
