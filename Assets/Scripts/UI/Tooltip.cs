using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text Text;
    public string Description;
    //bool _isHovering;

    /*
    // Update is called once per frame
    void Update()
    {
        if (_isHovering)
        {
            //Vector3 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //screenPoint += new Vector3(Text.preferredWidth/2.0f, 10.0f, 0.0f);
            //RectTransform.transform.localPosition = screenPoint;
            if (Input.GetMouseButtonDown(0))
            {
                HideTooltip();
            }
        }
    }*/

    public void OnPointerEnter(PointerEventData eventData)
    {
        //_isHovering = true;
        DisplayTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //_isHovering = false;
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
