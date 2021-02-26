using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectUI : BaseUI
{

    private const int MIN_WORLD = 1;
    private const int MAX_WORLD = 5;
    private const int LEVELS_PER_WORLD = 6;

    public GameObject title1;
    public GameObject title2;
    public GameObject title3;
    public GameObject title4;
    public GameObject title5;

    private int currentWorld = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        if (currentWorld == 1)
        {
            title1.SetActive(true);
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
    }

    public void GoToLevel(int levelNumber)
    {
        int levelOrderNumber = (currentWorld - 1) * LEVELS_PER_WORLD + levelNumber;
        LoadLevel(levelOrderNumber);
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
