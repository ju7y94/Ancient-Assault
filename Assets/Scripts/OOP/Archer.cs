using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ArcherAI { Idle, Alert, Retreat, Death, Patrol };
    public class Archer : EnemyH
{
    [SerializeField] GameObject arrow;
    ArcherAI enemyAI;


    protected override void CheckAIStates()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
        distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.transform.position);

        if(targetPlayer != null)
        playerH = targetPlayer.GetComponent<PlayerH>();
        else
        enemyAI = ArcherAI.Idle;

        if (isDead)
        {
            enemyAI = ArcherAI.Death;
        }

        switch(enemyAI)
        {
            case ArcherAI.Alert:
            
                if(targetPlayer != null && distanceToPlayer <= 6f && distanceToPlayer >= 2f)
                {
                    gameObject.transform.forward = targetPlayer.transform.position - transform.position;
                    animator.SetBool("walk", false);
                    animator.SetBool("attack", true);
                    navAgent.isStopped = true;
                }
                else if(targetPlayer != null && distanceToPlayer < 2f)
                {
                    enemyAI = ArcherAI.Retreat;
                }

                if (targetPlayer == null || distanceToPlayer > 6f)
                {
                    enemyAI = ArcherAI.Idle;
                    print("Alert to Idle");
                }
            break;

            case ArcherAI.Idle:

                animator.SetBool("walk", false);
                animator.SetBool("attack", false);
                animator.SetBool("idle", true);
                navAgent.isStopped = true;

                if (targetPlayer != null && distanceToPlayer <= 6f)
                {
                    enemyAI = ArcherAI.Alert;
                    print("Idle to Alert");
                }
            break;

            case ArcherAI.Death:

                animator.SetBool("die", true);
                navAgent.isStopped = true;

            break;

            case ArcherAI.Retreat:

                animator.SetBool("walk", true);
                animator.SetBool("attack", false);
                navAgent.isStopped = false;
                gameObject.transform.forward = targetPlayer.transform.position - transform.position;
                navAgent.SetDestination(transform.position + Vector3.back);


                if (targetPlayer != null && distanceToPlayer >= 2f)
                {
                    enemyAI = ArcherAI.Alert;
                }            

            break;
        }
    }

    protected virtual void ArrowShot()
    {
        GameObject arrowInstance = Instantiate(arrow, transform.position + Vector3.up / 2, transform.rotation);
        Rigidbody arrowInstanceRB = arrowInstance.GetComponent<Rigidbody>();
        if(targetPlayer != null)
        {
            arrowInstanceRB.AddForce((targetPlayer.transform.position - transform.position) * 2f, ForceMode.Impulse);
            Destroy(arrowInstance, 3f);
        }
        

    }

    protected override void AttackEnd()
    {
        ArrowShot();
        audioManager.AudioEnemyArrowShot();
    }
    public override void Damage(float damage)
    {
        base.Damage(damage);
    }
}


