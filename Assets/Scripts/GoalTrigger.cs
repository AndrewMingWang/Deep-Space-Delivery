using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    public static GoalTrigger Instance;

    [Header("Level Specifics")]
    public int NumPackages = 4;
    public int packagesDelivered = 0;
    public int packagesLost = 0;
    public bool levelCanEnd = true;

    private int optimalRemainingBudget;
    private int targetRemainingBudget;

    [Header("UI")]
    public GameObject ResultsPanel;
    public GameObject MenuPanel;
    public GameObject ActionsPanel;
    public GameObject GoalEffectPrefab;
    public ResultsPanelTypeEffect ResultsPanelTypeEffect;

    bool levelDoneAlready = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.EnrollSFXSource(GetComponent<AudioSource>());
        optimalRemainingBudget = MoneyManager.Instance.OptimalRemaining;
    }


    // Update is called once per frame
    void Update()
    {
        if (levelDoneAlready)
        {
            return;
        }

        // Finish level and bring up results panel
        if (IsLevelDone() && levelCanEnd)
        {
            FinishLevel();
            levelDoneAlready = true;
        }
    }

    private void FinishLevel()
    {
        // Hide UI
        // TODO: Make this an animation
        MenuPanel.SetActive(false);
        ActionsPanel.SetActive(false);

        int remainingBudget = MoneyManager.Instance.GetRemainingMoney();
        float percentPackagesDelivered = packagesDelivered / NumPackages * 100f;
        
        // Determining performance string
        int perfInt = DeterminePerformance(
            percentPackagesDelivered, 
            remainingBudget,
            optimalRemainingBudget
            );

        // Unlock next level
        if (percentPackagesDelivered >= 50 && remainingBudget >= 0)
        {
            int highestLevelUnlocked = PlayerPrefs.GetInt(LevelSelectUI.PLAYER_PREFS_HIGHEST_LEVEL_UNLOCKED, 0);
            int nextLevel = GetCurrentLevel() + 1;
            if (highestLevelUnlocked < nextLevel)
            {
                PlayerPrefs.SetInt(LevelSelectUI.PLAYER_PREFS_HIGHEST_LEVEL_UNLOCKED, nextLevel);
            }
        }

        ResultsPanelTypeEffect.SetIntroText(packagesDelivered, NumPackages, remainingBudget, perfInt);

        ResultsPanel.GetComponent<Animator>().SetBool("open", true);

        levelDoneAlready = true;
    }

    private int DeterminePerformance(
        float percentPackagesDelivered, 
        int remainingBudget,
        int optimalRemainingBudget
        )
    {
        if (percentPackagesDelivered < 50 || remainingBudget < 0)
        {
            return 0;
        } else if (percentPackagesDelivered < 100 && remainingBudget < optimalRemainingBudget)
        {
            return 1;
        } else if (percentPackagesDelivered < 100 && remainingBudget >= optimalRemainingBudget)
        {
            return 2;
        } else if (percentPackagesDelivered == 100 && remainingBudget < optimalRemainingBudget)
        {
            return 2;
        } else if (percentPackagesDelivered == 100 && remainingBudget >= optimalRemainingBudget)
        {
            return 3;
        }
        return -1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            GetComponent<AudioSource>().Play();
            packagesDelivered += 1;
            other.gameObject.SetActive(false);
            Instantiate(GoalEffectPrefab, transform.position, Quaternion.Euler(270.0f, 0.0f, 0.0f));
        }
    }

    public void ResetPlayerResults()
    {
        packagesDelivered = 0;
        packagesLost = 0;
        levelDoneAlready = false;
    }

    public bool IsLevelDone()
    {
        return packagesDelivered + packagesLost == NumPackages;
    }

    private int GetCurrentLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        int currentLevel = -1;
        if (int.TryParse(currentSceneName.Substring(5), out currentLevel))
        {
            return currentLevel;
        }
        return -1;
    }

}
