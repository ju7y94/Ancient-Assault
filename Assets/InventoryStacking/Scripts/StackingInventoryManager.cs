using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StackingInventoryManager : MonoBehaviour
{
    public static StackingInventoryManager Instance;
    public GameObject inventory;
    //public Transform cursor;
    //public Vector3 cursorOffset;
    public Transform inventorySlotHolder;
    public Transform inventoryHotbarSlotHolder;

    public List<bool> isFull;
    public List<Transform> slots;
    public List<Transform> hotbarSlots;
    public int currentSlot;

    private void Start() {
        InitializeInventory();
        SetSlotID();
        CheckSlots();
        Instance = this;
    }

    private void Update() {
        // if (inventory.activeSelf == true)
        // {
        //     cursor.position = Input.mousePosition + cursorOffset;
        // }
        // if(cursor.childCount > 0)
        // {
        //     cursor.gameObject.SetActive(true);
        // }
        // else
        // {
        //     cursor.gameObject.SetActive(false);
        // }
    }

    void InitializeInventory()
    {
        for (int i = 0; i < inventorySlotHolder.childCount; i++)
        {
            slots.Add(inventorySlotHolder.GetChild(i));
            isFull.Add(false);
        }
        for (int i = 0; i < inventoryHotbarSlotHolder.childCount; i++)
        {
            slots.Add(inventoryHotbarSlotHolder.GetChild(i));
            hotbarSlots.Add(inventoryHotbarSlotHolder.GetChild(i));
            isFull.Add(false);
        }
    }

    void SetSlotID()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if(slots[i].GetComponent<Slot>() != null)
            {
                slots[i].GetComponent<Slot>().ID = i;
            }
        }

    }

    public void CheckSlots()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if(slots[i].childCount>0)
            {
                isFull[i] = true;
            }
            else
            {
                isFull[i] = false;
            }
        }
    }

public void AddItem(GameObject item, int amount)
{
    CheckSlots();

    ItemController itemController = item.GetComponent<ItemController>();

    for (int i = 0; i < amount; i++)
    {
        bool added = false; // Initialize a flag to track whether an item has been added in this iteration.

        // First, check if an item with the same ID exists in the inventory and stack it.
        for (int j = 0; j < slots.Count; j++)
        {
            ItemController slotItemController = slots[j].GetComponentInChildren<ItemController>();

            if (slotItemController != null && slotItemController.item.id == itemController.item.id)
            {
                // Check if stacking is possible.
                int remainingSpace = slotItemController.item.maxStack - slotItemController.amount;
                if (remainingSpace > 0)
                {
                    // Calculate how much can be stacked without exceeding the max stack size.
                    int stackAmount = Mathf.Min(remainingSpace, amount - i);

                    // Update the slot's item amount and the item's UI (if applicable).
                    slotItemController.amount += stackAmount;
                    if (slotItemController.amount >= 2)
                        slotItemController.amountText.text = slotItemController.amount.ToString();

                    // Adjust the remaining amount to add.
                    i += stackAmount - 1;

                    // Exit the loop since we've stacked the item.
                    added = true;
                    break;
                }
            }
        }

        if (!added)
        {
            // If no matching item is found, add it to the first available slot.
            for (int j = 0; j < slots.Count; j++)
            {
                if (!isFull[j])
                {
                    // If the slot is empty, add the item.
                    Instantiate(item, slots[j]);
                    isFull[j] = true; // Mark the slot as full.
                    added = true; // Set the flag to true since we've added an item.
                    break; // Exit the loop since we've added an item.
                }
            }
        }

        if (!added)
        {
            Debug.Log("All slots are full");
            break; // Exit the outer loop since all slots are full.
        }
    }

    CheckSlots();
}
}
