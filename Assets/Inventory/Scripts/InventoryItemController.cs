using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemController : MonoBehaviour
{
    Item item;

    public UIVirtualButton removeButton;

    PlayerH playerH;
    PlayerWeapon playerWeapon;
    AudioManager audioManager;

    public void DropItem()
    {
        InventoryManager.Instance.Drop(item);
        Destroy(gameObject);
    }
    public void AddItem(Item newItem)
    {
        item = newItem;
    }
    public void UseItem()
    {
        playerH = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerH>();
        playerWeapon = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWeapon>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        switch (item.itemType)
        {
            case Item.ItemType.Potion:
                playerH.IncreaseHealth(item.value);
                break;
            case Item.ItemType.Book:
                playerH.IncreaseHealth(item.value);
                break;
            case Item.ItemType.Weapon:
                playerWeapon.IncreaseAmmunition(item.value);
                break;
        }
        audioManager.AudioWeaponPickUp();
        DropItem();
    }
}
