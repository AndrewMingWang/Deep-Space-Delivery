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
    public GameObject controlDisplay;

    private void Awake()
    {
        if (AudioManager.SFXOn)
        {
            soundButton.GetComponent<Image>().sprite = soundSpriteOn;
        }
        else
        {
            soundButton.GetComponent<Image>().sprite = soundSpriteOff;
        }
        if (AudioManager.MusicOn)
        {
            musicButton.GetComponent<Image>().sprite = musicSpriteOn;
        }
        else
        {
            musicButton.GetComponent<Image>().sprite = musicSpriteOff;
        }
    }

    public void ToggleMusic()
    {
        AudioManager.ToggleMusic();

        if (AudioManager.MusicOn)
        {
            musicButton.GetComponent<Image>().sprite = musicSpriteOn;
        } else
        {
            musicButton.GetComponent<Image>().sprite = musicSpriteOff;
        }
    }

    public void ToggleSound()
    {
        AudioManager.ToggleSFX();

        if (AudioManager.SFXOn)
        {
            soundButton.GetComponent<Image>().sprite = soundSpriteOn;
        } else
        {
            soundButton.GetComponent<Image>().sprite = soundSpriteOff;
        }
    }

    public void ViewControls()
    {
        if (!controlDisplay.activeSelf)
        {
            controlDisplay.SetActive(true);
        } else
        {
            controlDisplay.SetActive(false);
        }
    }

    public void ResetLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
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
