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

    [Header("Colors")]
    public Color UnlockedTextColor;
    public Color LockedTextColor;
    public Color ClearedTextColor;
    public Color UnlockedScreenColor;
    public Color LockedScreenColor;
    public Color ClearedScreenColor;

    public void SetLocked()
    {
        Text.color = LockedTextColor;
        Screen.color = LockedScreenColor;
        Text.font = NonGlow;
    }

    public void SetUnlocked()
    {
        Text.color = UnlockedTextColor;
        Screen.color = UnlockedScreenColor;
        Text.font = Glow;
    }

    public void SetCleared()
    {
        Text.color = ClearedTextColor;
        Screen.color = ClearedScreenColor;
        Text.font = Glow;
    }
}
