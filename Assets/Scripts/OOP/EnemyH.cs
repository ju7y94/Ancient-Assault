using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public enum EnemyAI { Idle, Alert, Retreat, Death };
public abstract class EnemyH : Human
{
    
    protected NavMeshAgent navAgent;
    protected GameObject targetPlayer;
    protected Animator animator;
    [SerializeField] protected Slider healthBar;
    protected bool isDead;
    protected EnemyManager enemyManager;
    protected AudioManager audioManager;
    private float attackTimer;
    private float attackDelay = 1.5f;
    private bool meleeEnemy;
    protected PlayerH playerH;
    protected float distanceToPlayer;
    [SerializeField] protected float damageToPlayer;
    // Start is called before the first frame update

    private void Start() {
        currentHealthPoints = maxHealth;
        isDead = false;
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        enemyManager = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>();
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        healthBar.value = currentHealthPoints / maxHealth;
        print("ENEMY HP: "+currentHealthPoints);
    }

    // Update is called once per frame
    void Update()
    {
        //healthBar.value = currentHealthPoints / maxHealth;
        attackTimer += Time.deltaTime;
        healthBar.transform.forward = GameObject.FindGameObjectWithTag("Player").transform.position - healthBar.transform.position;
        CheckAIStates();
    }

    public override void Damage(float damage)
    {
        base.Damage(damage);

        healthBar.value = currentHealthPoints / maxHealth;
        if (currentHealthPoints <= 0)
        {
            isDead = true;
            healthBar.gameObject.SetActive(false);
            GetComponent<CapsuleCollider>().enabled = false;
            audioManager.AudioEnemyDeath();
            gameObject.tag = "Untagged";
            Destroy(gameObject, 3.5f);
            enemyManager.Invoke("PlaceNewEnemy", 1f);
            enemyManager.SubtractEnemy();
        }
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    protected abstract void CheckAIStates();

    protected abstract void AttackEnd();
}



