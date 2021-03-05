using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroTextType : MonoBehaviour
{
    public TMP_Text DisplayText;
    public string Content;
    public float CharTypeSpeed;

    public void PlayIntroText()
    {
        StringUtility.TypeTextEffect(DisplayText, Content, CharTypeSpeed);
    }
}
