using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Music names
    public static readonly string MUSIC_MENU = "menu";
    public static readonly string MUSIC_WORLD1 = "world1";
    public static readonly string MUSIC_WORLD2 = "world2";

    public static AudioManager Instance;
    public static bool SFXOn = true;
    public static bool MusicOn = true;

    [HideInInspector]
    public AudioFile CurrentMusic = null;
    public string StartingSong;

    public List<AudioFile> MusicFiles = new List<AudioFile>();

    public List<AudioFile> SFXFiles = new List<AudioFile>();

    private void Awake()    
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // MusicFiles instantiated here, SFX per prefab
            foreach (AudioFile audioFile in MusicFiles)
            {
                audioFile.SetAudioSource(gameObject.AddComponent<AudioSource>());
                audioFile.GetAudioSource().clip = audioFile.clip;
                audioFile.GetAudioSource().volume = audioFile.volume;
            }
        }
        else
        {
            Destroy(gameObject);
        }

        // First song to be played
        AudioManager.PlayMusic(StartingSong);
    }

    public static void EnrollSFXSource(AudioSource source)
    {
        AudioFile audioFile = new AudioFile();
        audioFile.volume = source.volume;
        audioFile.SetAudioSource(source);

        if (Instance != null)
        {
            Instance.SFXFiles.Add(audioFile);
        }
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

    public static void PlayMusic(string audioName)
    {
        if (Instance != null)
        {
            Instance.PlayMusicIfExists(audioName);
        }
    }

    private void PlayMusicIfExists(string audioName)
    {
        AudioFile audioFile = MusicFiles.Find(audio => audio.name.Equals(audioName));
        if (audioFile == null)
        {
            return;
        }

        if (CurrentMusic.GetAudioSource() != null)
        {
            CurrentMusic.GetAudioSource().Stop();
        }
        CurrentMusic = audioFile;
        CurrentMusic.GetAudioSource().Play();
    }

    /*
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
    */

    public static void StopAllAudio()
    {
        if (Instance != null)
        {
            Instance.StopPlayingAllAudio();
        }
    }

    private void StopPlayingAllAudio()
    {
        foreach (AudioFile audio in MusicFiles)
        {
            audio.GetAudioSource().Stop();
        }
        foreach (AudioFile audio in SFXFiles)
        {
            audio.GetAudioSource().Stop();
        }
    }
}
