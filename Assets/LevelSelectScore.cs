using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelSelectScore : MonoBehaviour
{
    public TMP_Text text;
    public Image screen;

    [Header("Appearance")]
    public Color LockedColor;
    public Color LockedTextColor;
    public Color UnlockedColor;
    public Color UnlockedTextColor;
    public Color ScoredColor;
    public Color PerfectColor;
    public Color ScoreTextColor;
    public TMP_FontAsset YellowGlow;
    public TMP_FontAsset GreenGlow;
    public TMP_FontAsset NonGlow;

    public void SetScore(int score)
    {
        switch (score)
        {
            case -1: // LOCKED
                text.text = "locked";
                screen.color = LockedColor;
                text.color = LockedTextColor;
                text.font = NonGlow;
                break;
            case 0: // UNLOCKED
                text.text = "...";
                screen.color = UnlockedColor;
                text.color = UnlockedTextColor;
                text.font = GreenGlow;
                break;
            case 1:
                text.text = "okay";
                screen.color = ScoredColor;
                text.color = ScoreTextColor;
                text.font = YellowGlow;
                break;
            case 2:
                text.text = "good";
                screen.color = ScoredColor;
                text.color = ScoreTextColor;
                text.font = YellowGlow;
                break;
            case 3:
                text.text = "perfect";
                screen.color = PerfectColor;
                text.color = ScoreTextColor;
                text.font = YellowGlow;
                break;
            default:
                Debug.Log("Unknown score");
                break;
        }
    }
}
