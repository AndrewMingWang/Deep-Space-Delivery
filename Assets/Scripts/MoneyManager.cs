using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    public int StartingMoney = 100;
    public Item[] Items;

    public TMP_Text MoneyText;
    public TMP_Text WallText;
    public TMP_Text ArrowText;
    public TMP_Text HoldingText;

    private int moneySpent = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        DisplayRemainingMoney();
        foreach (Item item in Items)
        {
            if (item.name.Equals(BuildManager.WALL))
            {
                WallText.text = "Wall: $" + item.price;
            }
            else if (item.name.Equals(BuildManager.ARROW))
            {
                ArrowText.text = "Arrow: $" + item.price;
            }
            else if (item.name.Equals(BuildManager.HOLDING))
            {
                HoldingText.text = "Holding: $" + item.price;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DisplayRemainingMoney()
    {
        MoneyText.text = "$" + (StartingMoney - moneySpent);
        if (StartingMoney > moneySpent)
        {
            MoneyText.color = Color.green;
        } else if (StartingMoney < moneySpent)
        {
            MoneyText.color = Color.red;
        } else
        {
            MoneyText.color = Color.black;
        }
    }

    public void PurchaseItem(string itemName)
    {
        foreach (Item item in Items)
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
        foreach (Item item in Items)
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
        return StartingMoney - moneySpent;
    }

    public void ResetMoney()
    {
        moneySpent = 0;
        DisplayRemainingMoney();
    }

}
