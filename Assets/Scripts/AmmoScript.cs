using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoScript : MonoBehaviour
{
    [SerializeField] int damageAmount;

    private void OnCollisionEnter(Collision collision)
    {
        EnemyH enemyObject = collision.gameObject.GetComponent<EnemyH>();
        
        if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "PoisonedEnemy")
        {
            enemyObject.Damage(damageAmount);
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Platform")
        {
            Destroy(gameObject);
        }
        
    }
}
