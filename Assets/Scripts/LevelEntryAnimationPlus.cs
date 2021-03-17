using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEntryAnimationPlus : MonoBehaviour
{
    public Animator LevelEntryUI;
    public AudioSource StartSource;
    public AudioSource GoalSource;
    public AudioClip Landing;
    public AudioClip OpenStart;
    public AudioClip OpenGoal;

    private void Awake()
    {
        AudioManager.EnrollSFXSource(StartSource);
        AudioManager.EnrollSFXSource(GoalSource);
    }

    public void TriggerBringUpUI()
    {
        LevelEntryUI.SetTrigger("bringupui");
    }

    public void PlayStartLand()
    {
        StartSource.PlayOneShot(Landing);
    }

    public void PlayGoalLand()
    {
        GoalSource.PlayOneShot(Landing);
    }

    public void PlayOpenStart()
    {
        StartSource.PlayOneShot(OpenStart);
    }

    public void PlayOpenGoal()
    {
        GoalSource.PlayOneShot(OpenGoal);
    }
}
