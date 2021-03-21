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
    public AudioSource DialogueSource;
    public Animator FadeAnimator;
    public GameObject ContinueText;
    public bool ContinueShown = false;

    private void Awake()
    {
        StringUtility.Instance.IsTyping = true;
    }

    private void Update()
    {
        // If we skip, stop the intro sound
        if (StringUtility.Instance.ShouldSkip)
        {
            DialogueSource.Stop();
        }

        if (!StringUtility.Instance.IsTyping)
        {
            if (!ContinueShown)
            {
                ContinueText.SetActive(true);
                ContinueShown = true;
            }

            if (Input.anyKeyDown)
            {
                FadeAnimator.SetTrigger("out");
                StartCoroutine(GoToLevelSelect());
                ContinueText.SetActive(false);
            }
        }
    }

    private IEnumerator GoToLevelSelect()
    {
        yield return new WaitForSecondsRealtime(4.05f);
        SceneManager.LoadSceneAsync("LevelSelect");
    }

    public void PlayIntroText()
    {
        StringUtility.TypeTextEffect(DisplayText, Content + "\n\n(Left Mouse Click to continue...)", CharTypeSpeed);
    }
}
