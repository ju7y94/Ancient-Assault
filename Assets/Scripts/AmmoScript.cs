using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoScript : MonoBehaviour
{
    [SerializeField] int damageAmount;

    private void OnCollisionEnter(Collision collision)
    {
        EnemyScript enemyObject = collision.gameObject.GetComponent<EnemyScript>();
        // if(enemyObject != null)
        // {
        //     enemyObject.DealDamage(damageAmount);
        //     Destroy(gameObject);
        // }
        if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "PoisonedEnemy")
        {
            enemyObject.DealDamage(damageAmount);
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Platform")
        {
            Destroy(gameObject);
        }
        
    }
}
