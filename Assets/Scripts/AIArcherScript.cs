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
    PlayerHealth playerHealth;
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
        Collider[] objectsHit = Physics.OverlapBox(transform.position, Vector3.one * 8f, gameObject.transform.rotation, layerMask);
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

                if (Mathf.Abs(playerDistanceX) < 3f && (Mathf.Abs(playerDistanceZ) < 3f))
                {
                    aiState = EnemyState.Retreat;
                }
            }
        }

        if (enemyDead)
        {
            aiState = EnemyState.Death;
        }


        if (aiState == EnemyState.Alert)
        {
            if(targetPlayer != null)
            {
                gameObject.transform.forward = targetPlayer.transform.position - transform.position;
                animator.SetBool("walk", false);
                animator.SetBool("attack", true);
                navAgent.isStopped = true;
                //print("ARCHER ATTACK");
            }
        }
        else if (aiState == EnemyState.Idle)
        {
            animator.SetBool("walk", false);
            animator.SetBool("attack", false);
            navAgent.isStopped = true;
            //print("ARCHER IDLE");

        }
        else if (aiState == EnemyState.Death)
        {
            //audioManager.AudioArcherDead();
            animator.SetBool("die", true);
            navAgent.isStopped = true;
            //print("ARCHER DEAD");

        }
        else if (aiState == EnemyState.Retreat)
        {
            //print("ARCHER RETREAT");
            animator.SetBool("walk", true);
            animator.SetBool("attack", false);
            navAgent.isStopped = false;
            navAgent.SetDestination(transform.position - Vector3.forward);
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

