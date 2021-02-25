using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Music names
    public static readonly string MUSIC_MENU = "musicMenu";
    public static readonly string MUSIC_WORLD1 = "musicWorld1";
    public static readonly string MUSIC_WORLD2 = "musicWorld2";

    // SFX names
    public static readonly string SFX_REACH_GOAL = "reachGoal";

    public static AudioManager Instance;
    public static bool SFXOn = true;
    public static bool MusicOn = true;

    [HideInInspector]
    public AudioFile CurrentMusic = null;
    public string StartingSong;

    [Header("All Audio")]
    public AudioFile[] AudioFiles;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            foreach (AudioFile audioFile in AudioFiles)
            {
                audioFile.SetAudioSource(gameObject.AddComponent<AudioSource>());
                audioFile.GetAudioSource().clip = audioFile.clip;
                audioFile.GetAudioSource().loop = audioFile.isLoop;
                audioFile.GetAudioSource().volume = audioFile.volume;
            }
        }
        else
        {
            Destroy(gameObject);
        }

        AudioManager.Play(StartingSong);
    }

    public static void ToggleMusic()
    {
        if (Instance != null)
        {
            Instance.ToggleAllMusic();
        }
    }

    private void ToggleAllMusic()
    {
        MusicOn = !MusicOn;
        foreach (AudioFile af in AudioFiles)
        {
            if (af.isMusic)
            {
                if (MusicOn)
                {
                    af.Unmute();
                } else
                {
                    af.Mute();
                }                
            }
        }
    }

    public static void ToggleSFX()
    {
        if (Instance != null)
        {
            Instance.ToggleAllSFX();
        }
    }

    private void ToggleAllSFX()
    {
        SFXOn = !SFXOn;
        foreach (AudioFile af in AudioFiles)
        {
            if (!af.isMusic)
            {
                if (SFXOn)
                {
                    af.Unmute();
                }
                else
                {
                    af.Mute();
                }
            }
        }
    }

    public static void Play(string audioName)
    {
        if (Instance != null)
        {
            Instance.PlayIfExists(audioName);
        }
    }

    private void PlayIfExists(string audioName)
    {
        AudioFile audioFile = Array.Find(AudioFiles, audio => audio.name.Equals(audioName));
        if (audioFile == null)
        {
            return;
        }

        if (audioFile.isMusic)
        {
            // This is music - only one can play at a time
            if (CurrentMusic.GetAudioSource() != null)
            {
                CurrentMusic.GetAudioSource().Stop();
            }
            CurrentMusic = audioFile;

            CurrentMusic.GetAudioSource().Play();
        } else
        {
            // This is SFX
            audioFile.GetAudioSource().Play();
        }
    }

    public static void Stop(string audioName)
    {
        if (Instance != null)
        {
            Instance.StopIfExists(audioName);
        }
    }

    private void StopIfExists(string audioName)
    {
        AudioFile audioFile = Array.Find(AudioFiles, audio => audio.name.Equals(audioName));
        if (audioFile == null)
        {
            return;
        }

        audioFile.GetAudioSource().Stop();
    }

    public static void StopAllAudio()
    {
        if (Instance != null)
        {
            Instance.StopPlayingAllAudio();
        }
    }

    private void StopPlayingAllAudio()
    {
        foreach (AudioFile audio in AudioFiles)
        {
            audio.GetAudioSource().Stop();
        }
    }
}
