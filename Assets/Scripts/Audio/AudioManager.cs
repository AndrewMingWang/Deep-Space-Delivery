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

    [Header("Music")]
    public List<AudioFile> MusicFiles = new List<AudioFile>();

    [Header("SFX")]
    public List<AudioFile> SFXFiles = new List<AudioFile>();

    [HideInInspector]
    public List<AudioFile> AllFiles = new List<AudioFile>();

    private void Awake()
    {
        foreach (AudioFile af in MusicFiles)
        {
            AllFiles.Add(af);
        }
        foreach (AudioFile af in SFXFiles)
        {
            AllFiles.Add(af);
        }

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            foreach (AudioFile audioFile in AllFiles)
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

        // First song to be played
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
        foreach (AudioFile audioFile in MusicFiles)
        {
            if (MusicOn)
            {
                audioFile.Unmute();
            } else
            {
                audioFile.Mute();
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
        foreach (AudioFile audioFile in SFXFiles)
        {
            if (SFXOn)
            {
                audioFile.Unmute();
            }
            else
            {
                audioFile.Mute();
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
        AudioFile audioFile = AllFiles.Find(audio => audio.name.Equals(audioName));
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
        AudioFile audioFile = AllFiles.Find(audio => audio.name.Equals(audioName));
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
        foreach (AudioFile audio in AllFiles)
        {
            audio.GetAudioSource().Stop();
        }
    }
}
