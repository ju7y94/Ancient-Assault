using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum WarriorAI { Idle, Alert, Death };
    public class Warrior : EnemyH
{

    protected WarriorAI enemyAI;


    protected override void CheckAIStates()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
        distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.transform.position);

        if(targetPlayer != null)
        playerH = targetPlayer.GetComponent<PlayerH>();
        else
        enemyAI = WarriorAI.Idle;

        if (isDead)
        {
            enemyAI = WarriorAI.Death;
        }

        switch (enemyAI)
        {
            case WarriorAI.Alert:
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
                    enemyAI = WarriorAI.Idle;
                }

            break;

            case WarriorAI.Idle:
                animator.SetBool("walk", false);
                animator.SetBool("attack", false);
                animator.SetBool("idle", true);
                navAgent.isStopped = true;

                if(targetPlayer != null && distanceToPlayer <= 3f)
                {
                    animator.SetBool("idle", false);
                    enemyAI = WarriorAI.Alert;
                }
            break;
            
            case WarriorAI.Death:
                animator.SetBool("die", true);
                navAgent.isStopped = true;
            break;
        }
    }

    protected override void AttackEnd()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position + transform.up / 4, 0.3f, transform.forward, out hit, 1f))
        {
            print("Spherecast");
            if (hit.transform != null)
            { 
                if (hit.transform.gameObject.tag == "Player")
                {
                    print("HIT PLAYER");
                    bool block = playerH.GetBlocking();
                    if (!block)
                    {
                        print("Damage not blocked");
                        playerH.Damage(damageToPlayer);
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



