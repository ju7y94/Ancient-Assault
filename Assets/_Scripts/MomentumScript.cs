using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomentumScript : MonoBehaviour
{
    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
