using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableTitleScreenManager : MonoBehaviour
{
    public TitleScreenManager tsm;

    public void FinishFadeIn()
    {
        tsm.DoneFadeIn = true;
    }
}
