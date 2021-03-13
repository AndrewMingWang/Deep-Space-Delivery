using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultsPanelTypeEffect : MonoBehaviour
{
    public TMP_Text DisplayText;
    public string Content;
    public float CharTypeSpeed;

    private void Awake()
    {
        StringUtility.Instance.IsTyping = true;
    }

    public void SetIntroText(int deliveredPackages, int totalPackages, int remainingBudget, string performanceString)
    {
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

        Content += "performance:\\s.................\\n\\n\\p\\p\\p";
        Content += performanceString;
    }

    public void PlayIntroText()
    {
        StringUtility.TypeTextEffect(DisplayText, Content, CharTypeSpeed);
    }
}
