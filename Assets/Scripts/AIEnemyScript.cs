using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum EnemyState { Idle, Alert, Retreat, Death };

public class AIEnemyScript : MonoBehaviour
{
    EnemyState aiState;
    NavMeshAgent navAgent;
    GameObject targetPlayer;
    Animator animator;
    EnemyScript enemyScript;
    PlayerHealth playerHealth;
    AudioManager audioManager;
    float playerDistanceX;
    float playerDistanceY;
    float playerDistanceZ;
    [SerializeField] float damageToPlayer;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        aiState = EnemyState.Idle;
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyScript = GetComponent<EnemyScript>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckAIStates();
    }



    void CheckAIStates()
    {
        bool enemyDead = enemyScript.IsDead();
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
        float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.transform.position);

        if(targetPlayer != null)
        playerHealth = targetPlayer.GetComponent<PlayerHealth>();
        else
        aiState = EnemyState.Idle;

        if (enemyDead)
        {
            aiState = EnemyState.Death;
        }


        switch (aiState)
        {
            case EnemyState.Alert:
                if (distanceToPlayer < 0.75f)
                {
                    animator.SetBool("walk", false);
                    animator.SetBool("attack", true);
                    navAgent.isStopped = true;
                }
                else if (distanceToPlayer >= 0.75f)
                {
                    animator.SetBool("walk", true);
                    animator.SetBool("attack", false);
                    navAgent.isStopped = false;
                    navAgent.destination = targetPlayer.transform.position;
                    transform.forward = targetPlayer.transform.position - transform.position;
                }
                if (distanceToPlayer >= 5f)
                {
                    aiState = EnemyState.Idle;
                }

            break;

            case EnemyState.Idle:
                animator.SetBool("walk", false);
                animator.SetBool("attack", false);
                animator.SetBool("idle", true);
                navAgent.isStopped = true;

                if(targetPlayer != null && distanceToPlayer <= 3f)
                {
                    animator.SetBool("idle", false);
                    aiState = EnemyState.Alert;
                }
            break;
            
            case EnemyState.Death:
                animator.SetBool("die", true);
                navAgent.isStopped = true;
            break;
        }
    }

    public void AttackEnd()
    {
        print("Attack triggered");
        RaycastHit hit;
        if (Physics.SphereCast(transform.position + transform.up / 4, 0.3f, transform.forward, out hit, 1f))
        {
            print("Spherecast");
            if (hit.transform != null)
            { 
                if (hit.transform.gameObject.tag == "Player")
                {
                    print("HIT PLAYER");
                    bool block = playerHealth.GetBlocking();
                    if (!block)
                    {
                        print("Damage not blocked");
                        playerHealth.Damage(damageToPlayer);
                    }
                    else if (block)
                    {
                        print("Damage blocked");
                        audioManager.AudioShieldBlock();
                    }
                }
            }
        }
    }
}

