using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public int ID;
    StackingInventoryManager manager;
    private RectTransform rectTransform;
    //[SerializeField] Vector3 offset; // Adjust this offset as needed.

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<StackingInventoryManager>();
    }

    public void SetID()
    {
        manager.currentSlot = ID;
    }

public void OnDrop(PointerEventData eventData)
{
    GameObject dropped = eventData.pointerDrag;
    InvItemController invItemController = dropped.GetComponent<InvItemController>();

    if (transform.childCount == 0)
    {
        // The slot is empty, so drop the dragged item here.
        invItemController.parentAfterDrag = transform;
    }
    else if (transform.childCount > 0)
    {
        // The slot is not empty. Check if the item ID matches.

        InvItemController existingItemController = transform.GetChild(0).GetComponent<InvItemController>();

        if (existingItemController.item.id == invItemController.item.id)
        {
            // The item IDs match, stack the items.
            int remainingSpace = existingItemController.item.maxStack - existingItemController.amount;

            if (remainingSpace > 0)
            {
                int stackAmount = Mathf.Min(remainingSpace, invItemController.amount);
                existingItemController.amount += stackAmount;
                invItemController.amount -= stackAmount;

                // Update UI to reflect the stacked items.
                if (existingItemController.amount > 1)
                {
                    existingItemController.amountText.text = existingItemController.amount.ToString();
                }

                // If the amount of the dragged item reaches zero, destroy it.
                if (invItemController.amount <= 0)
                {
                    Destroy(dropped);
                }
            }
        }
        else
        {
            // The item IDs don't match, so swap the slots.
            Transform tempParent = invItemController.parentAfterDrag;
            int tempSiblingIndex = invItemController.transform.GetSiblingIndex();

            invItemController.parentAfterDrag = transform;
            invItemController.transform.SetParent(transform);
            invItemController.transform.SetSiblingIndex(0);

            // Move the existing item to the previous parent and sibling index.
            existingItemController.parentAfterDrag = tempParent;
            existingItemController.transform.SetParent(tempParent);
            existingItemController.transform.SetSiblingIndex(tempSiblingIndex);
        }
    }
}
}
