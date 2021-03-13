using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    [System.Serializable]
    public struct ItemUI
    {
        public GameObject Panel;
        public GameObject Button;
        public TMP_Text PriceText;
    }

    public static MoneyManager Instance;

    [Header("Level Properties")]
    public int StartingMoney;
    public int TargetRemaining;
    public int OptimalRemaining;
    private int _moneySpent = 0;
    private float _targetPercent;

    [Header("Building Properties")]
    public Item[] Items;

    [Header("UI")]
    public ItemUI[] ItemUis;
    public TMP_Text MoneyText;
    public RectTransform MoneyBarRT;
    public RectTransform TargetRT;
    public Animator MoneyTextAnimator;
    Vector2 MoneyBarRTMin;
    Vector2 MoneyBarRTMax;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;

        MoneyBarRTMin = MoneyBarRT.offsetMax;
        MoneyBarRTMax = Vector2.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        DisplayRemainingMoney();
        int itemCount = Items.Length;
        for (int i = 0; i < itemCount; i += 1)
        {
            GameObject itemButton = ItemUis[i].Button;
            TMP_Text priceText = ItemUis[i].PriceText;
            Item item = Items[i];

            if (!(item.unlimited)){
                itemButton.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = "x" + item.quantity.ToString();
            } else {
                itemButton.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().enabled = false;
            }
            itemButton.transform.GetChild(1).GetComponent<Image>().sprite = item.icon;
            priceText.text = "$" + item.price.ToString();
        }
        for (int i = itemCount; i < ItemUis.Length; i += 1)
        {
            ItemUis[i].Panel.SetActive(false);
        }

        // Position target indicator
        _targetPercent = (float) TargetRemaining / StartingMoney;
        TargetRT.offsetMax = Vector2.Lerp(MoneyBarRTMin, MoneyBarRTMax, _targetPercent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseItem(int itemId)
    {
        Item item = Items[itemId];
        if (item.price > GetRemainingMoney())
        {
            MoneyTextAnimator.SetTrigger("nomoney");
            return;
        } else if (item.quantity < 1 && !item.unlimited) {
            return;
        } else
        {
            _moneySpent += item.price;
            item.quantity -= 1;
            ItemUis[itemId].Button.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = "x" + (item.quantity);
        }
        DisplayRemainingMoney();
        BuildManager.Instance.BuildBuilding(item.name);
    }

    private void DisplayRemainingMoney()
    {
        int remainingMoney = GetRemainingMoney();
        if (remainingMoney <= 0)
        {
            MoneyText.text = "NO CASH";
        } else {
            if (remainingMoney >= 1000)
            {
                int thousands = remainingMoney / 1000;
                int ones = remainingMoney - thousands * 1000;
                string onesString = ones.ToString();
                if (ones >= 100)
                {
                    ;
                } else if (ones >= 10)
                {
                    onesString = "0" + onesString;
                } else if (ones >= 1)
                {
                    onesString = "00" + onesString;
                } else
                {
                    onesString = "000";
                }

                MoneyText.text = "$" + thousands.ToString() + "," + onesString;
            } else
            {
                MoneyText.text = "$" + remainingMoney.ToString();
            }
        }

        float t = (float)GetRemainingMoney() / StartingMoney;
        Vector2 energyBarRTCur = Vector2.Lerp(MoneyBarRTMin, MoneyBarRTMax, t);
        MoneyBarRT.offsetMax = energyBarRTCur;
    }

    public void RefundItem(string itemName)
    {
        for (int i = 0; i < Items.Length; i += 1)
        {
            if (Items[i].name.Equals(itemName))
            {
                GameObject ItemButton = ItemUis[i].Button;
                Items[i].quantity += 1;
                ItemButton.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = "x" + (Items[i].quantity);
                _moneySpent -= Items[i].price;
                break;
            }
        }
        DisplayRemainingMoney();
    }

    public int GetRemainingMoney()
    {
        return StartingMoney - _moneySpent;
    }

    public void ResetMoney()
    {
        _moneySpent = 0;
        DisplayRemainingMoney();
    }

    public void SFXButtonPress()
    {
        AudioManager.PlaySFX(AudioManager.UI_BUTTON_PRESS);
    }
}
