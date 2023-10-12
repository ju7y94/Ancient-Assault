using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum WarriorAI { Idle, Alert, Death, Patrol };
    public class Warrior : EnemyH
{

    protected WarriorAI enemyAI;
    [SerializeField] protected LayerMask playerMask;
    [SerializeField] protected LayerMask[] enemiesLayerMask;
    public static Warrior Instance;

    private void Awake() {
        Instance = this;
    }

    protected override void CheckAIStates()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
        distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.transform.position);

        if(targetPlayer != null)
        playerH = targetPlayer.GetComponent<PlayerH>();

        if (isDead)
        {
            enemyAI = WarriorAI.Death;
        }

        switch (enemyAI)
        {
            case WarriorAI.Alert:
                if (distanceToPlayer < 1.75f)
                {
                    animator.SetBool("walk", false);
                    animator.SetBool("attack", true);
                    navAgent.isStopped = true;

                    Vector3 directionToTarget = targetPlayer.transform.position - transform.position;
                    Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
                    transform.rotation = lookRotation;
                }
                else if (distanceToPlayer >= 1.75f)
                {
                    animator.SetBool("walk", true);
                    animator.SetBool("attack", false);
                    navAgent.isStopped = false;
                    navAgent.destination = targetPlayer.transform.position;
                    transform.forward = targetPlayer.transform.position - transform.position;
                }
                if (distanceToPlayer >= 15f)
                {
                    enemyAI = WarriorAI.Idle;
                }

            break;

            case WarriorAI.Idle:
                animator.SetBool("walk", false);
                animator.SetBool("attack", false);
                animator.SetBool("idle", true);
                navAgent.isStopped = true;

                if(targetPlayer != null && distanceToPlayer <= 6f)
                {
                    animator.SetBool("idle", false);
                    enemyAI = WarriorAI.Alert;
                }
                else
                {
                    Invoke("SwitchToPatrol", 3f);
                }
                
            break;
            
            case WarriorAI.Death:
                animator.SetBool("die", true);
                navAgent.isStopped = true;
            break;

            case WarriorAI.Patrol:
                animator.SetBool("walk", true);
                navAgent.isStopped = false;
                if(navAgent.remainingDistance <= navAgent.stoppingDistance)
                {
                    Vector3 point;
                    if (RandomPoint(transform.position, 15f, out point))
                    {
                        navAgent.SetDestination(point);
                    }
                }
                if(targetPlayer != null && distanceToPlayer <= 5f)
                {
                    animator.SetBool("walk", false);
                    animator.SetBool("idle", false);
                    enemyAI = WarriorAI.Alert;
                }
            break;
        }
    }

    protected override void AttackEnd()
    {
        RaycastHit hit;

        if(Physics.SphereCast(transform.position + transform.up, 0.35f, transform.forward, out hit, 0.75f, playerMask, QueryTriggerInteraction.UseGlobal))
        {
            playerH = targetPlayer.GetComponent<PlayerH>();
            bool block = playerH.GetBlocking();
            if (!block)
            {
            
                playerH.Damage(damageToPlayer);
            }
            else if (block)
            {
            
                audioManager.AudioShieldBlock();
            }
        }

        Collider[] inRange = Physics.OverlapSphere(this.transform.position, 10f, enemiesLayerMask[0]);
        foreach (Collider col in inRange)
        {
            col.GetComponent<Warrior>().SwitchToAlert();
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) 
        { 

            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    void SwitchToPatrol()
    {
        enemyAI = WarriorAI.Patrol;
    }
    public void SwitchToAlert()
    {
        enemyAI = WarriorAI.Alert;
    }
}



