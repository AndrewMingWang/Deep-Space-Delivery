using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUI : BaseUI
{

    public GameObject musicButton;
    public Sprite musicSpriteOn;
    public Sprite musicSpriteOff;
    public GameObject soundButton;
    public Sprite soundSpriteOn;
    public Sprite soundSpriteOff;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleMusic()
    {
        AudioManager.musicOn = !AudioManager.musicOn;
        if (AudioManager.musicOn)
        {
            AudioManager.Play(AudioManager.MUSIC_WORLD1);
            musicButton.GetComponent<Image>().sprite = musicSpriteOn;
        } else
        {
            AudioManager.StopAudio();
            musicButton.GetComponent<Image>().sprite = musicSpriteOff;
        }
    }

    public void ToggleSound()
    {
        AudioManager.soundOn = !AudioManager.soundOn;
        if (AudioManager.soundOn)
        {
            soundButton.GetComponent<Image>().sprite = soundSpriteOn;
        } else
        {
            soundButton.GetComponent<Image>().sprite = soundSpriteOff;
        }
    }

    public void ViewControls()
    {
        Debug.Log("VIEW CONTROLS");
    }

    public void QuitPlay()
    {
        GoToScene("LevelSelect");
    }

    public void NextLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        int currentLevel = 0;
        if (int.TryParse(currentSceneName.Substring(5), out currentLevel))
        {
            GoToScene("Level" + (currentLevel + 1));
        } else
        {
            GoToScene("Level1");
        }
    }

}
