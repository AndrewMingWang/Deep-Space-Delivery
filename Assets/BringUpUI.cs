using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringUpUI : MonoBehaviour
{
    public Animator LevelEntryUI;

    public void TriggerBringUpUI()
    {
        LevelEntryUI.SetTrigger("bringupui");
    }
}
