using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushrooms : MonoBehaviour
{
    GameObject personCollecting;
    PlayerInventory playerInventory;
    [SerializeField] bool goldenMushroom, silverMushroom;
    void OnCollisionEnter(Collision collision)
    {
        personCollecting = collision.gameObject;
        if(personCollecting.tag == "Player")
        {
            playerInventory = personCollecting.GetComponent<PlayerInventory>();
            CollectMushroom();
        }
    }

    void CollectMushroom()
    {
        Destroy(gameObject);
        if(goldenMushroom)
        {
            playerInventory.SetGoldenMushroom(goldenMushroom);
        }
        else if(silverMushroom)
        {
            playerInventory.SetSilverMushroom(silverMushroom);
        }
    }
}
