using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PriceTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text Text;
    public string Description;
    bool _isHovering;

    private void Start()
    {
        Description = "Budget until not optimal: $" + (MoneyManager.Instance.GetRemainingMoney() - MoneyManager.Instance.OptimalRemaining).ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Description = "Budget until not optimal: $" + (MoneyManager.Instance.GetRemainingMoney() - MoneyManager.Instance.OptimalRemaining).ToString();
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
