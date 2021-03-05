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
    public GameObject controlDisplay;
    public TMP_Text levelTitle;
    public TMP_Text levelInformation;

    private Dictionary<int, string> levelToInformation = new Dictionary<int, string>();
    private float alphaDecrease = 0.0f;

    private void Awake()
    {
        levelToInformation[0] = "Deep Space Delivery is a puzzle game where you guide dogs to retrieve packages. You want to maximize profit so try to save money when possible. Make sure to get all dogs to safety...";
        levelToInformation[1] = "Gaps will make you lose dogs";
        levelToInformation[2] = "";
        levelToInformation[3] = "Wind will push dogs in the direction they are blowing. If you move at a right angle to the wind, you will move diagonally. Walls block wind.";
        levelToInformation[4] = "Walls can be stacked.";
        levelToInformation[5] = "";
        levelToInformation[6] = "";
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
        levelInformation.text = levelToInformation[currentLevel];
        StartCoroutine(FadeLevelInformation());
    }

    private void Update()
    {
        levelInformation.alpha -= alphaDecrease * Time.deltaTime;
    }

    private IEnumerator FadeLevelInformation()
    {
        yield return new WaitForSecondsRealtime(10.0f);
        alphaDecrease = 0.1f;
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

    public void SFXButtonPress()
    {
        AudioManager.PlaySFX(AudioManager.UI_BUTTON_PRESS);
    }

}
