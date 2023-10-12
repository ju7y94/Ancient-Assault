using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> items = new List<Item>();
    public Transform itemContent;
    public GameObject inventoryItem;
    public Toggle enableDrop;
    public InventoryItemController[] inventoryItems;
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    public void Add(Item item)
    {
        items.Add(item);
    }
    public void Drop(Item item)
    {
        items.Remove(item);
    }
    public void ListItems()
    {
        CleanItemList();
        foreach (var item in items)
        {
            GameObject obj = Instantiate(inventoryItem, itemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<Text>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var dropItem = obj.transform.Find("DropButton").GetComponent<Button>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;

            if(enableDrop.isOn)
            {
                dropItem.gameObject.SetActive(true);
            }
        }
        SetInventoryItems();
    }

    public void CleanItemList()
    {
        foreach (Transform item in itemContent)
        {
            Destroy(item.gameObject);
        }
    }

    public void EnableItemsDrop()
    {
        if(enableDrop.isOn)
        {
            foreach (Transform item in itemContent)
            {
                item.Find("DropButton").gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Transform item in itemContent)
            {
                item.Find("DropButton").gameObject.SetActive(false);
            }
        }
    }

    public void SetInventoryItems()
    {
        inventoryItems = itemContent.GetComponentsInChildren<InventoryItemController>();
        for (int i=0; i<items.Count; i++)
        {
            inventoryItems[i].AddItem(items[i]);
        }
    }
}
