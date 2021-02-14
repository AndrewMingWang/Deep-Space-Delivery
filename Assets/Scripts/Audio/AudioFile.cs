using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioFile
{

    public string name;
    public AudioClip audioClip;
    public bool isMusic = false;
    [Range(0.0f, 1.0f)]
    public float volume = 0.5f;
    private AudioSource audioSource;

    public AudioSource GetAudioSource()
    {
        return audioSource;
    }

    public void SetAudioSource(AudioSource audioSource)
    {
        this.audioSource = audioSource;
    }

}
