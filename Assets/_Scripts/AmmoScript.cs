using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoScript : MonoBehaviour, IPooledObject
{
    [SerializeField] int damageAmount;

    public void OnObjectSpawn()
    {
        Rigidbody bulletRB = GetComponent<Rigidbody>();
        bulletRB.AddForce(transform.forward * 8f, ForceMode.Impulse);
        Invoke("DeactivateObject", 4f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        
        if(collision.gameObject.tag == "Warrior" || collision.gameObject.tag == "Archer")
        {
            EnemyH enemyObject = collision.gameObject.GetComponent<EnemyH>();
            enemyObject.Damage(damageAmount);
            
            //GetComponent<MeshCollider>().enabled = false;
            DeactivateObject();
        }
        else if(collision.gameObject.tag == "Platform")
        {
            DeactivateObject();
        }
        
    }

    void DeactivateObject()
    {
        gameObject.SetActive(false);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
    
}
