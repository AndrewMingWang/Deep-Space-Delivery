using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectUI : BaseUI
{

    private const int MIN_WORLD = 1;
    private const int MAX_WORLD = 5;
    private const int LEVELS_PER_WORLD = 6;
    public static readonly string PLAYER_PREFS_HIGHEST_LEVEL_UNLOCKED = "Level";

    public GameObject title1;
    public GameObject title2;
    public GameObject title3;
    public GameObject title4;
    public GameObject title5;
    public GameObject level0;
    public GameObject levelCleared0;
    public GameObject levelCleared1;
    public GameObject levelLocked1;
    public GameObject levelCleared2;
    public GameObject levelLocked2;
    public GameObject levelCleared3;
    public GameObject levelLocked3;
    public GameObject levelCleared4;
    public GameObject levelLocked4;
    public GameObject levelCleared5;
    public GameObject levelLocked5;
    public GameObject levelCleared6;
    public GameObject levelLocked6;

    private int currentWorld = 1;
    private int highestLevelUnlocked = 0;
    private int cheatCodeCount = 0;
    private int resetCodeCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        highestLevelUnlocked = PlayerPrefs.GetInt(PLAYER_PREFS_HIGHEST_LEVEL_UNLOCKED, 0);
        DisplayWorld();
    }

    // Update is called once per frame
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
        title1.SetActive(false);
        title2.SetActive(false);
        title3.SetActive(false);
        title4.SetActive(false);
        title5.SetActive(false);
        level0.SetActive(false);
        levelCleared0.SetActive(false);
        levelCleared1.SetActive(false);
        levelLocked1.SetActive(false);
        levelCleared2.SetActive(false);
        levelLocked2.SetActive(false);
        levelCleared3.SetActive(false);
        levelLocked3.SetActive(false);
        levelCleared4.SetActive(false);
        levelLocked4.SetActive(false);
        levelCleared5.SetActive(false);
        levelLocked5.SetActive(false);
        levelCleared6.SetActive(false);
        levelLocked6.SetActive(false);
        if (currentWorld == 1)
        {
            title1.SetActive(true);
            level0.SetActive(true);
            if (highestLevelUnlocked > 0)
            {
                levelCleared0.SetActive(true);
            }
        }
        else if (currentWorld == 2)
        {
            title2.SetActive(true);
        }
        else if (currentWorld == 3)
        {
            title3.SetActive(true);
        }
        else if (currentWorld == 4)
        {
            title4.SetActive(true);
        }
        else
        {
            title5.SetActive(true);
        }
        int baseLevel = (currentWorld - 1) * LEVELS_PER_WORLD;
        if (baseLevel + 1 < highestLevelUnlocked)
        {
            levelCleared1.SetActive(true);
        }
        else if (baseLevel + 1 > highestLevelUnlocked) 
        {
            levelLocked1.SetActive(true);
        }
        if (baseLevel + 2 < highestLevelUnlocked)
        {
            levelCleared2.SetActive(true);
        }
        else if (baseLevel + 2 > highestLevelUnlocked)
        {
            levelLocked2.SetActive(true);
        }
        if (baseLevel + 3 < highestLevelUnlocked)
        {
            levelCleared3.SetActive(true);
        }
        else if (baseLevel + 3 > highestLevelUnlocked)
        {
            levelLocked3.SetActive(true);
        }
        if (baseLevel + 4 < highestLevelUnlocked)
        {
            levelCleared4.SetActive(true);
        }
        else if (baseLevel + 4 > highestLevelUnlocked)
        {
            levelLocked4.SetActive(true);
        }
        if (baseLevel + 5 < highestLevelUnlocked)
        {
            levelCleared5.SetActive(true);
        }
        else if (baseLevel + 5 > highestLevelUnlocked)
        {
            levelLocked5.SetActive(true);
        }
        if (baseLevel + 6 < highestLevelUnlocked)
        {
            levelCleared6.SetActive(true);
        }
        else if (baseLevel + 6 > highestLevelUnlocked)
        {
            levelLocked6.SetActive(true);
        }
    }

    public void GoToLevel(int levelNumber)
    {
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

    public void SFXButtonPress()
    {
        AudioManager.PlaySFX(AudioManager.UI_BUTTON_PRESS);
    }

}
