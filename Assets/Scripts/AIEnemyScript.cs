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
    bool playerAlive;
    [SerializeField] float damageToPlayer;
    [SerializeField] LayerMask layerMask;
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
        playerAlive = TheGameManager.playerAlive;
        CheckAIStates();
    }



    void CheckAIStates()
    {
        bool enemyDead = enemyScript.IsDead();
        Collider[] objectsHit = Physics.OverlapBox(transform.position, Vector3.one * 5f, gameObject.transform.rotation, layerMask);
        foreach (Collider col in objectsHit)
        {

            if (col.gameObject.tag == "Player")
            {
                playerHealth = col.gameObject.GetComponent<PlayerHealth>();
                targetPlayer = col.gameObject;
                aiState = EnemyState.Alert;
                //print("GOT THE PLAYER");
                playerDistanceX = targetPlayer.transform.position.x - transform.position.x;
                playerDistanceY = targetPlayer.transform.position.y - transform.position.y;
                playerDistanceZ = targetPlayer.transform.position.z - transform.position.z;
                if (Mathf.Abs(playerDistanceX) > 6f && (Mathf.Abs(playerDistanceZ) > 6f))
                {
                    targetPlayer = null;
                }
            }

        }

        if (enemyDead)
        {
            aiState = EnemyState.Death;
        }




        if (aiState == EnemyState.Alert)
        {
            if (targetPlayer != null)
            {
                animator.SetBool("walk", true);
                animator.SetBool("attack", false);
                navAgent.isStopped = false;
                navAgent.destination = targetPlayer.transform.position;
                transform.forward = targetPlayer.transform.position - transform.position;
                //print("CHASING THE PLAYER");
                //Chasing
            }

            else
            {
                aiState = EnemyState.Idle;
            }

            if (Mathf.Abs(playerDistanceX) < 0.75f && Mathf.Abs(playerDistanceY) < 1f && (Mathf.Abs(playerDistanceZ) < 0.75f))
            {
                animator.SetBool("walk", false);
                animator.SetBool("attack", true);
                navAgent.isStopped = true;
                //print("ATTACKING THE PLAYER");
                //Attacking
            }
            /*else if (targetPlayer == null)
            {
                aiState = EnemyState.Idle;
                //print("LOST THE PLAYER");
                //Lost the player
            }*/
        }
        else if (aiState == EnemyState.Idle)
        {
            animator.SetBool("walk", false);
            animator.SetBool("attack", false);
            navAgent.isStopped = true;
            //Idle();
        }
        else if (aiState == EnemyState.Death)
        {
            //audioManager.AudioEnemyDeath();
            animator.SetBool("die", true);
            navAgent.isStopped = true;
            //Death();
        }
    }



    public bool EnemyGroundCheck()
    {
        float distanceToGround = 1f;
        return Physics.Raycast(transform.position, Vector3.down, distanceToGround);
    }

    public void AttackEnd()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position + transform.up / 4, 0.2f, transform.forward, out hit, 1f))
        {
            if (hit.transform != null)
            { 
                if (hit.transform.gameObject.tag == "Player")
                {
                    bool block = playerHealth.GetBlocking();
                    if (!block)
                    {
                        
                        
                            playerHealth.Damage(damageToPlayer);
                        

                        
                    }
                    else if (block)
                    {
                        audioManager.AudioShieldBlock();
                    }
                }
            }
        }
        /*if (targetPlayer != null)
        {
            bool block = playerHealth.GetBlocking();
            if (!block)
            {
                if (playerAlive)
                {
                    playerHealth.Damage(damageToPlayer);
                }

                //audioManager.AudioPlayerHurt();
            }
            else if (block)
            {
                audioManager.AudioShieldBlock();
            }

        }*/
    }
}

