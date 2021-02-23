using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private RectTransform tooltipView;
    private TMP_Text tooltipText;
    private Dictionary<string, string> elementNameToDescription = new Dictionary<string, string>();

    // Start is called before the first frame update
    void Start()
    {
        tooltipView = GameObject.FindGameObjectWithTag("tooltip").GetComponent<RectTransform>();
        tooltipText = tooltipView.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        elementNameToDescription.Add("wall", "buildable tile that blocks objects");
        elementNameToDescription.Add("arrow", "change direction of units");
        elementNameToDescription.Add("holding", "hold units until threshold reached");
        elementNameToDescription.Add("trampoline", "launch units up and forward");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPoint = Input.mousePosition;
        screenPoint += new Vector3(tooltipText.preferredWidth / 2.0f, 10.0f, 0.0f);
        tooltipView.transform.position = screenPoint;
        if (Input.GetMouseButtonDown(0))
        {
            tooltipView.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string elementName = eventData.pointerEnter.transform.GetChild(2).GetComponent<TMP_Text>().text;
        DisplayTooltip(elementName);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }

    private void DisplayTooltip(string elementName)
    {
        string description = elementNameToDescription[elementName];
        tooltipText.text = description;
        float padding = 10.0f;
        tooltipView.sizeDelta = new Vector2(tooltipText.preferredWidth + padding, tooltipText.preferredHeight + padding);
        tooltipView.gameObject.SetActive(true);
    }

    private void HideTooltip()
    {
        tooltipView.gameObject.SetActive(false);
    }

}
