using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static readonly string SOUND_REACH_GOAL = "reachGoal";
    public static readonly string MUSIC_WORLD1 = "world1";

    public static AudioManager instance;
    public static bool soundOn = true;
    public static bool musicOn = true;

    public AudioFile[] audioFiles;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            foreach (AudioFile audioFile in audioFiles)
            {
                audioFile.SetAudioSource(gameObject.AddComponent<AudioSource>());
                audioFile.GetAudioSource().clip = audioFile.audioClip;
                audioFile.GetAudioSource().loop = audioFile.isMusic;
                audioFile.GetAudioSource().volume = audioFile.volume;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Play(string audioName)
    {
        if (instance != null)
        {
            instance.PlayIfExists(audioName);
        }
    }

    private void PlayIfExists(string audioName)
    {
        AudioFile audioFile = Array.Find(audioFiles, audio => audio.name.Equals(audioName));
        if (audioFile == null)
        {
            return;
        }
        if ((audioFile.isMusic && musicOn) || (!audioFile.isMusic && soundOn))
        {
            if (audioFile.isMusic)
            {
                StopAudio();
            }
            audioFile.GetAudioSource().Play();
        }
    }

    public static void StopAudio()
    {
        if (instance != null)
        {
            instance.StopPlayingAudio();
        }
    }

    private void StopPlayingAudio()
    {
        foreach (AudioFile audio in audioFiles)
        {
            audio.GetAudioSource().Stop();
        }
    }
}
