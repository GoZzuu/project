using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour  {

    public List<ItemHelper> inventoryItemHelpers = new List<ItemHelper>();

    public GameObject uiItemPrefab;


    private void OnEnable()
    {
        Item.ItemCollectedEventHandler += AddItem;
    }
    private void OnDisable()
    {
        Item.ItemCollectedEventHandler -= AddItem;
    }


    public void AddItem(Item item)
    {
        int count = 1;

        for (int i = 0; i < inventoryItemHelpers.Count; i++)
        {
            if(inventoryItemHelpers[i].Item == item)
            {
                // add counts of items
                inventoryItemHelpers[i].AddCount(count);
                Debug.Log(item.name + " collected! Count "+ inventoryItemHelpers[i].Count);
                return;
            }
        }

        if (item.Useable)
        {
            inventoryItemHelpers.Add(new ItemHelper(item,
            InstatiateButton(item), count
            ));
        }
        else
        {
            inventoryItemHelpers.Add(new ItemHelper(item, count
            ));
        }

        Debug.Log(item.name + " collected!");
    }
    Button InstatiateButton(Item item)
    {
        var button = Instantiate(uiItemPrefab, transform).GetComponent<Button>();
        button.name = item.name;

        if (item.Icon != null)
            button.image.sprite = item.Icon;

        button.onClick.AddListener(delegate
        {
            UseItem(item);
        });

        return button;
    }


    public void UseItem(Item item)
    {
        for (int i = 0; i < inventoryItemHelpers.Count; i++)
        {
            if (inventoryItemHelpers[i].Item == item)
            {
                item.Use(PlayerStats.instance);
                inventoryItemHelpers[i].RemoveOne();

                if (inventoryItemHelpers[i].Count == 0)
                {
                    Destroy(inventoryItemHelpers[i].Button.gameObject);
                    inventoryItemHelpers.Remove(inventoryItemHelpers[i]);
                }

                return;
            }
        }

        Debug.Log("No items");
    }
}

[Serializable]
public class ItemHelper
{
    public Item Item;
    public int Count;
    public Button Button;

    Text CountText;

    public ItemHelper(Item item, Button button, int count = 1)
    {
        this.Item = item;
        this.Button = button;

        CountText = Button.GetComponentInChildren<Text>();
        SetCount(count);
    }
    public ItemHelper(Item item, int count = 1)
    {
        this.Item = item;
        Count = count;
    }

    public void SetCount(int count)
    {
        Count = count;
        CountText.text = Count.ToString();
    }
    public void RemoveOne()
    {
        Count -= 1;
        CountText.text = Count.ToString();
    }
    public void AddCount(int count)
    {
        Count += count;
        CountText.text = Count.ToString();
    }
    /*
    public static bool operator ==(ItemHelper helper, Item item) => helper.item == item;
    public static bool operator !=(ItemHelper helper, Item item) => helper.item != item;*/
}
