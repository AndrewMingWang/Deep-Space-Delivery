using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text Text;
    public string Description;
    public string Type;

    private void Awake()
    {
        if (Type.ToLower().Equals("trampoline"))
        {
            Description = "launches units forwards and upwards";
        }
        else if (Type.ToLower().Equals("holding"))
        {
            Description = "gathers units so they move as one";
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DisplayTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }

    private void DisplayTooltip()
    {
        Text.text = Description;
    }

    private void HideTooltip()
    {
        Text.text = "";
    }

}
