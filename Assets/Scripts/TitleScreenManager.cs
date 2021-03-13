using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    // Purely for debugging
    public bool DontActuallyGoToIntro;
    public bool DoneFadeIn = false;

    public Animator FadeAnimator;

    private void Update()
    {
        if (Input.anyKeyDown && DoneFadeIn)
        {
            if (DontActuallyGoToIntro)
            {
                FadeAnimator.SetTrigger("out");
                Debug.Log("any key pressed!");
                return;
            }
            FadeAnimator.SetTrigger("out");
            StartCoroutine(GoToIntro());
        }
    }

    private IEnumerator GoToIntro()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadSceneAsync("Intro");
    }
}
