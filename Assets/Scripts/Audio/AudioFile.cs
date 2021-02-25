using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioFile
{

    public string name;
    public AudioClip clip;
    public bool isLoop = false;
    public bool isMusic = false;
    [Range(0.0f, 1.0f)]
    public float volume = 0.5f;

    private AudioSource source;

    public AudioSource GetAudioSource()
    {
        return source;
    }

    public void SetAudioSource(AudioSource audioSource)
    {
        this.source = audioSource;
    }

    public void Mute()
    {
        this.source.volume = 0;
    }

    public void Unmute()
    {
        this.source.volume = volume;
    }

}
