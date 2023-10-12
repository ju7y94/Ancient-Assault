using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemController : MonoBehaviour
{
    public Item item;
    public int amount = 1;
    public Text amountText;
    public GameObject itemIcon;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StackingInventoryManager.Instance.AddItem(itemIcon, gameObject.GetComponent<ItemController>().amount);
            Destroy(gameObject);
        }
    }
}

