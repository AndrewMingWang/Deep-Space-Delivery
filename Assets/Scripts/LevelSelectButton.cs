using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectButton : MonoBehaviour
{
    public Image Screen;
    public TMP_Text Text;
    public TMP_FontAsset Glow;
    public TMP_FontAsset NonGlow;
    public TMP_FontAsset YellowGlow;
    public RectTransform ScoreLevelRT;
    Vector2 ScoreLevelRTMin;
    Vector2 ScoreLevelRTMax;

    [Header("Colors")]
    public Color UnlockedTextColor;
    public Color LockedTextColor;
    public Color ClearedTextColor;
    public Color UnlockedScreenColor;
    public Color LockedScreenColor;
    public Color ClearedScreenColor;

    private void Awake()
    {
        ScoreLevelRTMax = ScoreLevelRT.offsetMax;
        ScoreLevelRTMin = ScoreLevelRT.offsetMin;
    }

    public void SetLocked()
    {
        SetScore(0);
        Text.color = LockedTextColor;
        Screen.color = LockedScreenColor;
        Text.font = NonGlow;
    }

    public void SetUnlocked()
    {
        SetScore(0);
        Text.color = UnlockedTextColor;
        Screen.color = UnlockedScreenColor;
        Text.font = Glow;
    }

    public void SetScore(int score)
    {
        // 3 -> 0; 2 -> 1/3; 1 -> 2/3; 0 -> 1;
        float ratio = (3f - score) / 3f;
        Vector2 set = ScoreLevelRTMax * ratio;
        set.x = 0;
        ScoreLevelRT.offsetMax = set;

        if (score == 0)
        {
            Text.color = UnlockedTextColor;
            Screen.color = UnlockedScreenColor;
            Text.font = Glow;
        } else
        {
            Text.color = ClearedTextColor;
            Screen.color = ClearedScreenColor;
            Text.font = YellowGlow;
        }
    }

    public void SetLevelNumber(int num)
    {
        Text.text = num.ToString();
        if (num >= 20)
        {
            Text.fontSize = 38;
            Text.alignment = TextAlignmentOptions.Center;
        } else if (num >= 10)
        {
            Text.fontSize = 38;
            Text.alignment = TextAlignmentOptions.Left;
        } else
        {
            Text.fontSize = 40;
            Text.alignment = TextAlignmentOptions.Center;
        }
    }
}
