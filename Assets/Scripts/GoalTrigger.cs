using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    public static GoalTrigger Instance;

    public static readonly int NUM_PLAYERS = 4;

    public GameObject ResultsPanel;
    public TMP_Text SuccessRate;
    public TMP_Text MoneyLeft;
    public GameObject Star1;
    public GameObject Star2;
    public GameObject Star3;
    public GameObject Fail;
    public GameObject MenuPanel;
    public GameObject ActionsPanel;
    public GameObject GoalEffectPrefab;

    public int PlayersReached = 0;
    public int PlayersFailed = 0;

    bool levelDoneAlready = false;
    public bool levelEndEnabled = true;

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

        if (IsLevelDone() && levelEndEnabled)
        {
            MenuPanel.SetActive(false);
            ActionsPanel.SetActive(false);
            MoneyManager moneyManager = GameObject.FindGameObjectWithTag("moneyManager").GetComponent<MoneyManager>();
            moneyManager.MoneyText.gameObject.SetActive(false);
            int moneyLeft = moneyManager.GetRemainingMoney();
            float successRate = (PlayersReached * 100.0f / NUM_PLAYERS);
            ResultsPanel.GetComponent<Animator>().SetBool("open", true);
            SuccessRate.text = successRate + "%";
            MoneyLeft.text = moneyLeft.ToString();
            Fail.SetActive(false);
            Star1.SetActive(false);
            Star2.SetActive(false);
            Star3.SetActive(false);
            if (successRate < 50.0f || moneyLeft < 0)
            {
                Fail.SetActive(true);
                AudioManager.PlaySFX(AudioManager.UI_LOSE_LEVEL);
            }
            else
            {
                int highestLevelUnlocked = PlayerPrefs.GetInt(LevelSelectUI.PLAYER_PREFS_HIGHEST_LEVEL_UNLOCKED, 0);
                int nextLevel = GetCurrentLevel() + 1;
                if (highestLevelUnlocked < nextLevel)
                {
                    PlayerPrefs.SetInt(LevelSelectUI.PLAYER_PREFS_HIGHEST_LEVEL_UNLOCKED, nextLevel);
                }
            }
            // TODO: Animate the stars popping up
            if (successRate >= 50.0f && successRate < 100.0f && moneyLeft == 0)
            {
                Star1.SetActive(true);
                AudioManager.PlaySFX(AudioManager.UI_WIN_LEVEL);
            }
            else if (successRate >= 50.0f && successRate < 100.0f && moneyLeft > 0)
            {
                Star1.SetActive(true);
                Star2.SetActive(true);
                AudioManager.PlaySFX(AudioManager.UI_WIN_LEVEL);
            }
            else if (successRate == 100.0f && moneyLeft == 0)
            {
                Star1.SetActive(true);
                Star2.SetActive(true);
                AudioManager.PlaySFX(AudioManager.UI_WIN_LEVEL);
            }
            else if (successRate == 100.0f && moneyLeft > 0)
            {
                Star1.SetActive(true);
                Star2.SetActive(true);
                Star3.SetActive(true);
                AudioManager.PlaySFX(AudioManager.UI_WIN_LEVEL);
            }
            levelDoneAlready = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            GetComponent<AudioSource>().Play();
            PlayersReached += 1;
            other.gameObject.SetActive(false);
            Instantiate(GoalEffectPrefab, transform.position, Quaternion.Euler(270.0f, 0.0f, 0.0f));
        }
    }

    public void ResetPlayerResults()
    {
        PlayersReached = 0;
        PlayersFailed = 0;
        levelDoneAlready = false;
    }

    public bool IsLevelDone()
    {
        return PlayersReached + PlayersFailed == NUM_PLAYERS;
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
