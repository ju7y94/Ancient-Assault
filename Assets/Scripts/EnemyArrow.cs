using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    [SerializeField] float damageToPlayer;
    bool block;
    AudioManager audioManager;
    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }
    private void OnCollisionEnter(Collision collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            PlayerHealth playerHealth = collider.gameObject.GetComponent<PlayerHealth>();
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
        Destroy(gameObject);
    }
}
