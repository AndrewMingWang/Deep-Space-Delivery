using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    public const string FAIL_STRING = "fail";
    public const string SUCCESS1_STRING = "satisfactory";
    public const string SUCCESS2_STRING = "good";
    public const string SUCCESS3_STRING = "excellent";
    public const string UNKNOWN_STRING = "unknown";
    public const int TEMP_OPTIMAL_BUDGET = 2000;

    public static GoalTrigger Instance;

    public static readonly int NUM_PACKAGES = 4;

    public GameObject ResultsPanel;
    public GameObject MenuPanel;
    public GameObject ActionsPanel;
    public GameObject GoalEffectPrefab;
    public ResultsPanelTypeEffect ResultsPanelTypeEffect;

    public int packagesDelivered = 0;
    public int packagesLost = 0;

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
    }


    // Update is called once per frame
    void Update()
    {
        if (levelDoneAlready)
        {
            return;
        }

        // Finish level and bring up results panel
        if (IsLevelDone())
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
        float percentPackagesDelivered = packagesDelivered / NUM_PACKAGES * 100f;
        
        // Determining performance string
        string performanceString = DeterminePerformance(percentPackagesDelivered, remainingBudget, TEMP_OPTIMAL_BUDGET);

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

        ResultsPanelTypeEffect.SetIntroText(packagesDelivered, NUM_PACKAGES, remainingBudget, TEMP_OPTIMAL_BUDGET, performanceString);

        ResultsPanel.GetComponent<Animator>().SetBool("open", true);

        levelDoneAlready = true;
    }

    private string DeterminePerformance(float percentPackagesDelivered, int remainingBudget, int optimalRemainingBudget)
    {
        if (percentPackagesDelivered < 50 || remainingBudget < 0)
        {
            return FAIL_STRING;
        } else if (percentPackagesDelivered < 100 && remainingBudget < optimalRemainingBudget)
        {
            return SUCCESS1_STRING;
        } else if (percentPackagesDelivered < 100 && remainingBudget >= optimalRemainingBudget)
        {
            return SUCCESS2_STRING;
        } else if (percentPackagesDelivered == 100 && remainingBudget < optimalRemainingBudget)
        {
            return SUCCESS2_STRING;
        } else if (percentPackagesDelivered == 100 && remainingBudget >= optimalRemainingBudget)
        {
            return SUCCESS3_STRING;
        }
        return UNKNOWN_STRING;
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
        return packagesDelivered + packagesLost == NUM_PACKAGES;
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
