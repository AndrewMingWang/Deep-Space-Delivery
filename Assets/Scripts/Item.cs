using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string name;
    public int price;
    public int quantity;

    public bool unlimited = false;
    public Sprite icon;
}
