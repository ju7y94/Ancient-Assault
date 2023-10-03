using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIArcherScript : MonoBehaviour
{
    EnemyState aiState;
    NavMeshAgent navAgent;
    GameObject targetPlayer;
    [SerializeField] GameObject arrow;
    Animator animator;
    EnemyScript enemyScript;
    AudioManager audioManager;

    [SerializeField] LayerMask layerMask;

    float playerDistanceX;
    float playerDistanceY;
    float playerDistanceZ;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        aiState = EnemyState.Idle;
        targetPlayer = TheGameManager.GetPlayerInstance();
        
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
        //print(distanceToPlayer);

        if (targetPlayer == null)
        aiState = EnemyState.Idle;

        if (enemyDead)
        {
            aiState = EnemyState.Death;
        }

        switch(aiState)
        {
            case EnemyState.Alert:
            
                if(targetPlayer != null && distanceToPlayer <= 6f && distanceToPlayer >= 2f)
                {
                    gameObject.transform.forward = targetPlayer.transform.position - transform.position;
                    animator.SetBool("walk", false);
                    animator.SetBool("attack", true);
                    navAgent.isStopped = true;
                }

                else if(targetPlayer != null && distanceToPlayer < 2f)
                {
                    aiState = EnemyState.Retreat;
                }

                if (targetPlayer == null || distanceToPlayer > 6f)
                {
                    aiState = EnemyState.Idle;
                    print("Alert to Idle");
                }
            break;

            case EnemyState.Idle:

                animator.SetBool("walk", false);
                animator.SetBool("attack", false);
                animator.SetBool("idle", true);
                navAgent.isStopped = true;

                if (targetPlayer != null && distanceToPlayer <= 6f)
                {
                    aiState = EnemyState.Alert;
                    print("Idle to Alert");
                }
            break;

            case EnemyState.Death:

                animator.SetBool("die", true);
                navAgent.isStopped = true;

            break;

            case EnemyState.Retreat:

                animator.SetBool("walk", true);
                animator.SetBool("attack", false);
                navAgent.isStopped = false;
                navAgent.SetDestination(transform.position - Vector3.forward);


                if (targetPlayer != null && distanceToPlayer >= 2f)
                {
                    aiState = EnemyState.Alert;
                }            

            break;
        }
    }


    void ArrowShot()
    {
        GameObject arrowInstance = Instantiate(arrow, transform.position + Vector3.up / 2, transform.rotation);
        Rigidbody arrowInstanceRB = arrowInstance.GetComponent<Rigidbody>();
        if(targetPlayer != null)
        {
            arrowInstanceRB.AddForce((targetPlayer.transform.position - transform.position) * 2f, ForceMode.Impulse);
            Destroy(arrowInstance, 3f);
        }
        

    }
    public void AttackEnd()
    {
        ArrowShot();
        audioManager.AudioEnemyArrowShot();
    }
}

