using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InvItemController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    Canvas canvas;
    public Item item; // Your item data structure.
    public int amount = 1; // Current item amount.
    public Text amountText; // Text to display item amount.

    private bool isDraggingItem = false;
    private GameObject draggedItem;
    public int originalSlotIndex;
    [HideInInspector] public Transform parentAfterDrag;


    public void OnBeginDrag(PointerEventData eventData)
    {
        originalSlotIndex = GetComponentInParent<Slot>().ID;
        Debug.Log("Original slot index "+originalSlotIndex);
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        eventData.pointerDrag.GetComponent<Image>().raycastTarget = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        eventData.pointerDrag.GetComponent<Image>().raycastTarget = true;
    }
}
