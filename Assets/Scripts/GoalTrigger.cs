using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{

    public static readonly int NUM_PLAYERS = 4;

    public GameObject ResultsPanel;
    public TMP_Text SuccessRate;
    public TMP_Text MoneyLeft;
    public GameObject Star1;
    public GameObject Star2;
    public GameObject Star3;
    public GameObject Fail;
    public GameObject MenuButtons;
    public GameObject ActionButtons;
    public GameObject EnergyBar;

    public int PlayersReached = 0;
    public int PlayersFailed = 0;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.EnrollSFXSource(GetComponent<AudioSource>());
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLevelDone())
        {
            MenuButtons.SetActive(false);
            ActionButtons.SetActive(false);
            EnergyBar.SetActive(false);
            MoneyManager moneyManager = GameObject.FindGameObjectWithTag("moneyManager").GetComponent<MoneyManager>();
            moneyManager.MoneyText.text = "";
            int moneyLeft = moneyManager.GetRemainingMoney();
            float successRate = (PlayersReached * 100.0f / NUM_PLAYERS);
            ResultsPanel.GetComponent<Animator>().SetTrigger("open");
            SuccessRate.text = successRate + "%";
            MoneyLeft.text = moneyLeft.ToString();
            if (successRate < 50.0f || moneyLeft < 0)
            {
                Fail.SetActive(true);
            }
            else if (successRate >= 50.0f && successRate < 100.0f && moneyLeft == 0)
            {
                Star1.SetActive(true);
            }
            else if (successRate >= 50.0f && successRate < 100.0f && moneyLeft > 0)
            {
                Star1.SetActive(true);
                Star2.SetActive(true);
            }
            else if (successRate == 100.0f && moneyLeft == 0)
            {
                Star1.SetActive(true);
                Star2.SetActive(true);
            }
            else if (successRate == 100.0f && moneyLeft > 0)
            {
                Star1.SetActive(true);
                Star2.SetActive(true);
                Star3.SetActive(true);
            }
            else
            {
                Debug.LogError("Unknown score");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            GetComponent<AudioSource>().Play();
            PlayersReached += 1;
            other.gameObject.SetActive(false);
        }
    }

    public void ResetPlayerResults()
    {
        PlayersReached = 0;
        PlayersFailed = 0;
    }

    public bool IsLevelDone()
    {
        return PlayersReached + PlayersFailed == NUM_PLAYERS;
    }

}
