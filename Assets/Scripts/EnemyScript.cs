using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] float health;
    float maxHealth = 100;
    [SerializeField] float damageToPlayer;
    PlayerHealth playerHealth;
    float attackTimer;
    float attackDelay = 1.5f;
    AudioManager audioManager;
    EnemyManager enemyManager;
    bool dead;
    [SerializeField] Slider healthBar;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerHealth = other.gameObject.GetComponent<PlayerHealth>();

            if (attackTimer >= attackDelay)
            {
                if (!playerHealth.GetBlocking())
                {
                    //playerHealth.Damage(damageToPlayer);
                    attackTimer = 0f;
                }
                else if (playerHealth.GetBlocking())
                {
                    attackTimer = 0f;
                }
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        dead = false;
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        enemyManager = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        healthBar.transform.forward = GameObject.FindGameObjectWithTag("Player").transform.position - healthBar.transform.position;
    }

    public void DealDamage(float damageAmount)
    {
        health -= damageAmount;
        healthBar.value = health / maxHealth;
        if (health <= 0)
        {
            dead = true;
            audioManager.AudioEnemyDeath();
            gameObject.tag = "Untagged";
            Destroy(gameObject, 3.5f);
            enemyManager.Invoke("PlaceNewEnemy", 1f);
            enemyManager.SubtractEnemy();
        }
    }

    public bool IsDead()
    {
        return dead;
    }
}
