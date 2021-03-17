using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEntryAnimationPlus : MonoBehaviour
{
    public Animator LevelEntryUI;

    public void TriggerBringUpUI()
    {
        LevelEntryUI.SetTrigger("bringupui");
    }

    public void PlayStartLand()
    {
        AudioManager.PlaySFX(AudioManager.SFX_LAND_PORTAL);
    }

    public void PlayGoalLand()
    {
        AudioManager.PlaySFX(AudioManager.SFX_LAND_PORTAL);
    }

    public void PlayOpenStart()
    {
        AudioManager.PlaySFX(AudioManager.SFX_OPEN_START);
    }

    public void PlayOpenGoal()
    {
        AudioManager.PlaySFX(AudioManager.SFX_OPEN_GOAL);
    }
}
