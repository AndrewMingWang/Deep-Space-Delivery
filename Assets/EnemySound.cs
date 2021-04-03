using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    public AudioSource Source;
    public AudioClip Alert;
    public AudioClip Collide;

    private void Awake()
    {
        AudioManager.EnrollSFXSource(Source);
    }
    public void PlayAlert()
    {
        Source.PlayOneShot(Alert);
    }

    public void PlayCollide()
    {
        Source.PlayOneShot(Collide);
    }
}
