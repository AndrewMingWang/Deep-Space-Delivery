using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{

    public static readonly int NUM_PLAYERS = 20;

    public GameObject ResultsPanel;
    public TMP_Text SuccessRate;
    public TMP_Text MoneyLeft;
    public TMP_Text Verdict;

    public int PlayersReached = 0;
    public int PlayersFailed = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLevelDone())
        {
            MoneyManager moneyManager = GameObject.FindGameObjectWithTag("moneyManager").GetComponent<MoneyManager>();
            moneyManager.MoneyText.text = "";
            int moneyLeft = moneyManager.GetRemainingMoney();
            float successRate = (PlayersReached * 100.0f / NUM_PLAYERS);
            ResultsPanel.GetComponent<Animator>().SetTrigger("open");
            SuccessRate.text = successRate + "%";
            MoneyLeft.text = "$" + moneyLeft;
            if (moneyLeft < 0)
            {
                Verdict.text = "F";
            }
            else if (moneyLeft > 0 && successRate == 100.0f)
            {
                Verdict.text = "A+";
            }
            else if (successRate == 100.0f)
            {
                Verdict.text = "A";
            }
            else if (successRate >= 80.0f)
            {
                Verdict.text = "B";
            }
            else if (successRate >= 65.0f)
            {
                Verdict.text = "C";
            }
            else if (successRate >= 50.0f)
            {
                Verdict.text = "D";
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            StartCoroutine(ManageAudioSource(other.GetComponent<AudioSource>(), this.gameObject));
            PlayersReached += 1;
            other.gameObject.SetActive(false);
        }
    }

    public bool IsLevelDone()
    {
        return PlayersReached + PlayersFailed == NUM_PLAYERS;
    }

    public IEnumerator ManageAudioSource(AudioSource info, GameObject goal)
    {   
        GameObject GoalAudioSource = new GameObject();
        GoalAudioSource.transform.parent = goal.transform;
        GoalAudioSource.name = "NewGoalAudioSource";
        GoalAudioSource.AddComponent<AudioSource>();
        AudioSource newAudio = GoalAudioSource.GetComponent<AudioSource>();
        newAudio.clip = info.clip;
        newAudio.pitch = (Random.Range(0.8f, 1.2f));
        newAudio.Play();
        while(newAudio.isPlaying){
            yield return null;
        }
        Destroy(GoalAudioSource);

    }

}
