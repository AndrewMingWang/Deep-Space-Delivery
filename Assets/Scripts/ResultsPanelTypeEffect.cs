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

    public void SetIntroText(int deliveredPackages, int totalPackages, int spentOverBudget, int perfInt)
    {
        DisplayText.text = "";
        Content = string.Format(
            "delivered:{0}{1}/{2}\\n\\p\\p",
            new string(' ', 16 - deliveredPackages.ToString().Length - totalPackages.ToString().Length - 1),
            deliveredPackages,
            totalPackages
            );
        if (perfInt > 0 && spentOverBudget > 0)
        {
            Content += string.Format(
                "over optimal:{0}${1}\\n",
                new string(' ', 13 - spentOverBudget.ToString().Length - 1),
                spentOverBudget
                );
        }
        Content += "\\p\\p\\o";
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
