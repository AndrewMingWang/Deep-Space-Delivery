using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;

    public int startingMoney = 100;
    public Item[] items;

    public TMP_Text moneyText;
    public TMP_Text wallText;
    public TMP_Text arrowText;

    private int moneySpent = 0;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        DisplayRemainingMoney();
        foreach (Item item in items)
        {
            if (item.name.Equals(BuildManager.WALL))
            {
                wallText.text = "Wall: $" + item.price;
            }
            else if (item.name.Equals(BuildManager.ARROW))
            {
                arrowText.text = "Arrow: $" + item.price;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DisplayRemainingMoney()
    {
        moneyText.text = "$" + (startingMoney - moneySpent);
        if (startingMoney > moneySpent)
        {
            moneyText.color = Color.green;
        } else if (startingMoney < moneySpent)
        {
            moneyText.color = Color.red;
        } else
        {
            moneyText.color = Color.black;
        }
    }

    public void PurchaseItem(string itemName)
    {
        foreach (Item item in items)
        {
            if (itemName.Equals(item.name))
            {
                moneySpent += item.price;
            }
        }
        DisplayRemainingMoney();
    }

    public void RefundItem(string itemName)
    {
        foreach (Item item in items)
        {
            if (itemName.Equals(item.name))
            {
                moneySpent -= item.price;
            }
        }
        DisplayRemainingMoney();
    }

    public int GetRemainingMoney()
    {
        return startingMoney - moneySpent;
    }

    public void ResetMoney()
    {
        moneySpent = 0;
        DisplayRemainingMoney();
    }

}
