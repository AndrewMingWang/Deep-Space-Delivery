using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InterLevelTextType : MonoBehaviour
{
    public TMP_Text DisplayText;
    public string Content;
    public float CharTypeSpeed;
    public AudioSource DialogueSource;
    public Animator FadeAnimator;
    public string LevelName;

    private bool _textFlag = true;

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
        if (!StringUtility.Instance.IsTyping && _textFlag){
            GetComponent<AudioSource>().Stop();
            _textFlag = false;
        }

        if (!StringUtility.Instance.IsTyping)
        {

            if (Input.anyKeyDown)
            {
                FadeAnimator.SetTrigger("out");
                StartCoroutine(GoToLevel(LevelName));
            }
        }
    }

    private IEnumerator GoToLevel(string levelName)
    {
        yield return new WaitForSecondsRealtime(2.05f);
        SceneManager.LoadSceneAsync(levelName);
    }

    public void PlayIntroText()
    {
        StringUtility.TypeTextEffect(DisplayText, Content, CharTypeSpeed);
    }
}
