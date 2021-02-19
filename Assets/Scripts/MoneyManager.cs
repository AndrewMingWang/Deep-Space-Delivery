using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{

    public static MoneyManager Instance;

    private const int NUMBER_ITEMS = 5;

    public int StartingMoney = 100;
    public Item[] Items;

    public GameObject ItemButton1;
    public GameObject ItemButton2;
    public GameObject ItemButton3;
    public GameObject ItemButton4;
    public GameObject ItemButton5;
    private GameObject[] ItemButtons = new GameObject[10];
    // delete
    public TMP_Text MoneyText;
    public TMP_Text WallText;
    public TMP_Text ArrowText;
    public TMP_Text HoldingText;
    public TMP_Text TrampolineText;

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
        ItemButtons[0] = ItemButton1;
        ItemButtons[1] = ItemButton2;
        ItemButtons[2] = ItemButton3;
        ItemButtons[3] = ItemButton4;
        ItemButtons[4] = ItemButton5;
        int itemCount = Items.Length;
        for (int i = 0; i < itemCount; i += 1)
        {
            GameObject itemButton = ItemButtons[i];
            Item item = Items[i];
            itemButton.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = "x" + item.quantity.ToString();
            itemButton.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "$" + item.price.ToString();
            itemButton.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = item.name;
        }
        for (int i = itemCount; i < NUMBER_ITEMS; i += 1)
        {
            ItemButtons[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseItem(int itemId)
    {
        string itemName = ItemButtons[itemId].transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text;
        int itemQuantity = int.Parse(ItemButtons[itemId].transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text.Substring(1));
        int itemPrice = int.Parse(ItemButtons[itemId].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text.Substring(1));
        if (itemPrice > GetRemainingMoney() || itemQuantity < 1)
        {
            return;
        } else
        {
            moneySpent += itemPrice;
            ItemButtons[itemId].transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = "x" + (itemQuantity - 1);
        }
        DisplayRemainingMoney();
        BuildManager.Instance.BuildBuilding(itemName);
    }

    private void DisplayRemainingMoney()
    {
        MoneyText.text = "$" + GetRemainingMoney();
        if (StartingMoney > moneySpent)
        {
            MoneyText.color = Color.green;
        } else if (StartingMoney < moneySpent)
        {
            MoneyText.color = Color.red;
        } else
        {
            MoneyText.color = Color.yellow;
        }
    }

    public void RefundItem(string itemName)
    {
        for (int i = 0; i < ItemButtons.Length; i += 1)
        {
            GameObject ItemButton = ItemButtons[i];
            if (ItemButton != null && ItemButton.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text.Equals(itemName))
            {
                int itemQuantity = int.Parse(ItemButton.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text.Substring(1));
                ItemButton.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = "x" + (itemQuantity + 1);
                int itemPrice = int.Parse(ItemButton.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text.Substring(1));
                moneySpent -= itemPrice;
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
