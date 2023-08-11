using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBurnPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().SetPoisoned();
        }
    }
}
