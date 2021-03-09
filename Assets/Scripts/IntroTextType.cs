using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroTextType : MonoBehaviour
{
    public TMP_Text DisplayText;
    public string Content;
    public float CharTypeSpeed;

    private void Awake()
    {
        StringUtility.Instance.IsTyping = true;
    }

    private void Update()
    {
        if (!StringUtility.Instance.IsTyping)
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadSceneAsync("LevelSelect");
            }
        }
    }

    public void PlayIntroText()
    {
        StringUtility.TypeTextEffect(DisplayText, Content, CharTypeSpeed);
    }
}
