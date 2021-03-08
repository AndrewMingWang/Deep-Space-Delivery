using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColour : MonoBehaviour
{

    public Color afterColor;

    public void updateColour(){
        this.GetComponent<Image>().color = afterColor;
    }
}
