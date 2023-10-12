using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour, IPooledObject
{
    [SerializeField] float damageToPlayer;
    bool block;
    AudioManager audioManager;
    public void OnObjectSpawn()
    {
        Invoke("DeactivateObject", 4f);
    }
    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }
    private void OnCollisionEnter(Collision collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            PlayerH playerHealth = collider.gameObject.GetComponent<PlayerH>();
            block = playerHealth.GetBlocking();
            if(!block)
            {
                playerHealth.Damage(damageToPlayer);
                
            }
            else if(block)
            {
                audioManager.AudioShieldBlock();
            }
        }
        //DeactivateObject();
    }
    void DeactivateObject()
    {
        gameObject.SetActive(false);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
