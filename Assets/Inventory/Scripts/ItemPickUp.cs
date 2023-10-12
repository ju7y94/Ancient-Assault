using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item item;

    public void PickUp()
    {
        InventoryManager.Instance.Add(item);
        //Debug.Log("PICKED UP ITEM " + item.itemName);
        Destroy(gameObject);
    }


    // private void OnMouseDown()
    // {
    //     PickUp();
    // }
    // private void OnTriggerEnter(Collider collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         print("COLLISION");
    //         StackingInventoryManager.Instance.AddItem(gameObject, gameObject.GetComponent<ItemController>().amount);
    //         //PickUp();
    //     }
    // }
}
