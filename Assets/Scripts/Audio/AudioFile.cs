﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioFile
{

    public string name;
    public AudioClip clip;
    [Range(0.0f, 1.0f)]
    public float volume = 0.5f;

    public AudioSource source;

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
        if (this.source != null)
        {
            this.source.volume = 0;
        }
    }

    public void Unmute()
    {
        if (this.source != null)
        {
            this.source.volume = volume;
        }
    }

    // Should only be used for looping SFX
    public void Pause()
    {
        if (this.source != null)
        {
            this.source.Stop();
        }
    }

    public void Unpause()
    {
        if (this.source != null && this.source.isActiveAndEnabled)
        {
            this.source.Play();
        }
    }

}
