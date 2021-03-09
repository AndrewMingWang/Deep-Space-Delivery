using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelSelectUI : BaseUI
{
    private const int MIN_WORLD = 1;
    private const int MAX_WORLD = 5;
    private const int LEVELS_PER_WORLD = 6;
    public static readonly string PLAYER_PREFS_HIGHEST_LEVEL_UNLOCKED = "Level";

    public TMP_Text WorldTitle;
    public int[] LevelStates = new int[LEVELS_PER_WORLD + 1]; // 0 - locked, 1 - unlocked, 2 - cleared
    public LevelSelectButton[] LevelSelectButtons = new LevelSelectButton[LEVELS_PER_WORLD + 1];
    public LevelSelectButton DownWorldButton;
    public LevelSelectButton UpWorldButton;

    private int currentWorld = 1;
    private int highestLevelUnlocked = 0;
    private int cheatCodeCount = 0;
    private int resetCodeCount = 0;

    private void Awake()
    {
        highestLevelUnlocked = PlayerPrefs.GetInt(PLAYER_PREFS_HIGHEST_LEVEL_UNLOCKED, 0);
        DisplayWorld();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            cheatCodeCount += 1;
            if (cheatCodeCount >= 3)
            {
                highestLevelUnlocked = 10;
                DisplayWorld();
            }
        } else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            resetCodeCount += 1;
            if (resetCodeCount >= 3)
            {
                highestLevelUnlocked = 0;
                PlayerPrefs.SetInt(PLAYER_PREFS_HIGHEST_LEVEL_UNLOCKED, highestLevelUnlocked);
                DisplayWorld();
            }
        }
    }

    public void GoUpWorld()
    {
        currentWorld += 1;
        if (currentWorld > MAX_WORLD)
        {
            currentWorld = MIN_WORLD;
        }
        DisplayWorld();
    }

    public void GoDownWorld()
    {
        currentWorld -= 1;
        if (currentWorld < MIN_WORLD)
        {
            currentWorld = MAX_WORLD;
        }
        DisplayWorld();
    }

    public void DisplayWorld()
    {
        // Set the world text
        switch (currentWorld)
        {
            case 1:
                WorldTitle.text = "1. Plains";
                break;
            case 2:
                WorldTitle.text = "2. Night Plains";
                break;
            case 3:
                WorldTitle.text = "3. Ruins";
                break;
            case 4:
                WorldTitle.text = "4. Night Ruins";
                break;
            case 5:
                WorldTitle.text = "5. Station";
                break;
        }

        // Unlock tutorial
        if (currentWorld == 1)
        {
            if (highestLevelUnlocked >= 1)
            {
                LevelSelectButtons[0].SetCleared();
            } else
            {
                LevelSelectButtons[0].SetUnlocked();
            }
            
        }

        // Unlock levels
        int baseLevel = (currentWorld - 1) * LEVELS_PER_WORLD;

        for (int i = 1; i <= LEVELS_PER_WORLD; i++)
        {
            if (baseLevel + i < highestLevelUnlocked)
            {
                LevelSelectButtons[i].SetCleared();
            } else if (baseLevel + i == highestLevelUnlocked)
            {
                LevelSelectButtons[i].SetUnlocked();
            } else
            {
                LevelSelectButtons[i].SetLocked();
            }
        }

        // Set World Navigation Buttons
        DownWorldButton.SetUnlocked();
        UpWorldButton.SetUnlocked();
    }

    public void GoToLevel(int levelNumber)
    {
        StartCoroutine(GoToLevelAfterPause(levelNumber));
    }

    private IEnumerator GoToLevelAfterPause(int levelNumber)
    {
        yield return new WaitForSeconds(0.5f);
        if (levelNumber == 0)
        {
            LoadLevel(0);
        }

        int levelOrderNumber = (currentWorld - 1) * LEVELS_PER_WORLD + levelNumber;
        if (highestLevelUnlocked >= levelOrderNumber)
        {
            LoadLevel(levelOrderNumber);
        }
    }


    public void QuitGame()
    {
        Debug.Log("QUIT GAME");
        Application.Quit();
    }

    public void SFXButtonPressed(int levelNumber)
    {
        if (levelNumber == 0)
        {
            SFXButtonPressSuccess();
            return;
        }

        int baseLevel = (currentWorld - 1) * LEVELS_PER_WORLD;
        if (baseLevel + levelNumber > highestLevelUnlocked)
        {
            SFXButtonPressFail();
        } else
        {
            SFXButtonPressSuccess();
        }
    }

    public void SFXButtonPressSuccess()
    {
        AudioManager.PlaySFX(AudioManager.UI_BUTTON_PRESS);
    }

    public void SFXButtonPressFail()
    {
        AudioManager.PlaySFX(AudioManager.UI_CANNOT_BUILD);
    }

}
