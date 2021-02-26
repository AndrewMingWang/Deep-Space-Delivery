using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    private void Start()
    {
        AudioManager.EnrollSFXSource(GetComponent<AudioSource>());
        GetComponent<AudioSource>().Play();
    }
}
