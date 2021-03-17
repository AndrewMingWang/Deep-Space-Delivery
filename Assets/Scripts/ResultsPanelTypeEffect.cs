using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultsPanelTypeEffect : MonoBehaviour
{
    public static ResultsPanelTypeEffect Instance;

    [Header("Type Animator Options")]
    public TMP_Text DisplayText;
    public string Content;
    public int PerfInt;
    public float CharTypeSpeed;

    [Header("Performance Text Animation")]
    public Animator TextAnimator;
    bool _pastKeyFrameOne = false;
    bool _pastNotIsTyping = false;

    public List<TMP_Text> PerfTextBackgrounds = new List<TMP_Text>();
    public List<TMP_Text> PerfTextGlow = new List<TMP_Text>();

    private void Update()
    {
        if (StringUtility.Instance.KeyFrameOne && !_pastKeyFrameOne)
        {
            _pastKeyFrameOne = true;
            TextAnimator.SetTrigger("fadein");
            StringUtility.Instance.KeyFrameOne = false;
        }

        if (!StringUtility.Instance.IsTyping && !_pastNotIsTyping && _pastKeyFrameOne)
        {
            _pastNotIsTyping = true;
            PerfTextGlow[PerfInt].gameObject.SetActive(true);
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    public void SetIntroText(int deliveredPackages, int totalPackages, int remainingBudget, int perfInt)
    {
        DisplayText.text = "";
        Content = string.Format(
            "packages delivered:{0}{1}/{2}\\n\\n\\p\\p",
            new string(' ', 9 - deliveredPackages.ToString().Length - totalPackages.ToString().Length),
            deliveredPackages,
            totalPackages
            );
        Content += string.Format(
            "unspent budget:{0}${1}\\n\\n\\p\\p",
            new string(' ', 13 - remainingBudget.ToString().Length),
            remainingBudget
            );

        Content += "performance:\\o\\s.................\\n\\n\\p\\p\\p";
        PerfInt = perfInt;
    }

    public void PlayIntroText()
    {
        StringUtility.TypeTextEffect(DisplayText, Content, CharTypeSpeed);
    }

    public void Reset()
    {
        _pastKeyFrameOne = false;
        _pastNotIsTyping = false;

        TextAnimator.SetTrigger("fadeout");
        foreach (TMP_Text t in PerfTextGlow)
        {
            t.gameObject.SetActive(false);
        }
    }
}
