using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public Animator ControlsAnimator;
    // public GameObject informationDisplay;
    // public TMP_Text levelInformation;
    // private bool _levelInformationRecieved
    public TMP_Text levelTitle;
    public Animator UIAnimator;

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
        string currentSceneName = SceneManager.GetActiveScene().name;
        int currentLevel = -1;
        if (int.TryParse(currentSceneName.Substring(5), out currentLevel))
        {
            string buffer = "";
            if (currentLevel < 10)
            {
                buffer = "0";
            }
            levelTitle.text = "Level " + buffer + currentLevel;
        }
    }

    private void Update()
    {
        // levelInformation.alpha -= alphaDecrease * Time.deltaTime;
    }

    // public void StartLevelInformation(){
    //     if (!(_levelInformationRecieved)){
    //         StringUtility.TypeTextEffect(levelInformation, levelToInformation[_currentLevel], 1.0f);
    //         // StartCoroutine(FadeLevelInformation());
    //         _levelInformationRecieved = true;
    //     }
        
    // }

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
        ControlsAnimator.SetBool("show", !ControlsAnimator.GetBool("show"));
    }

    public void ResetLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void QuitPlayAfterPause()
    {
        StartCoroutine(QuitPlay());
    }

    public IEnumerator QuitPlay()
    {
        UIAnimator.SetTrigger("out");
        yield return new WaitForSecondsRealtime(2.1f);
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

    public void SFXButtonPress()
    {
        AudioManager.PlaySFX(AudioManager.UI_BUTTON_PRESS);
    }

}
