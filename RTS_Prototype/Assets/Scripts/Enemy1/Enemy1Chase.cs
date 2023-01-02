using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Chase : IState
{
    Enemy1 enemy1;

    public Enemy1Chase(Enemy1 enemy1)
    {
        this.enemy1 = enemy1;
    }

    public void Enter()
    {
        enemy1.enemy1NavMeshAgent.isStopped = false;
        enemy1.anim.SetInteger("animState", 2);
        enemy1.enemy1NavMeshAgent.speed += enemy1.chaseSpeedIncrease; 
    }

    public void Execute()
    {
        FindClosestEnemy();

        decideAction();
    }

    public void Exit()
    {
        enemy1.enemy1NavMeshAgent.speed -= enemy1.chaseSpeedIncrease;
    }





    private void decideAction()
    {
        Vector3 distanceBetwixt = Vector3.zero;

        if (enemy1.closestEnemy != null)
        {
            distanceBetwixt = enemy1.closestEnemy.transform.position - enemy1.transform.position;
        }

        if (enemy1.selected.health <= 0) //dead
        {
            enemy1.enemy1Machine.ChangeState(enemy1.dieState);
            return;
        }
        else if (enemy1.closestEnemy == null) //no units in range 
        {
            enemy1.enemy1Machine.ChangeState(enemy1.patrolState);
            return;
        }
        else if (distanceBetwixt.magnitude <= enemy1.attackRadius) //unit in attackable range
        {
            enemy1.enemy1Machine.ChangeState(enemy1.attackState);
            return;
        }
        else //chase enemy
        {
            enemy1.enemy1NavMeshAgent.SetDestination(enemy1.closestEnemy.transform.position);
        }
    }

    private void FindClosestEnemy()
    {
        //update enemies near list
        enemy1.getUnitsInRange(enemy1.unitsInRange);

        //detection radius is the max distance objects will be, add 10 to get edge cases
        float closestDistance = enemy1.detectionRadius + 10f;

        bool enemyDetected = false;

        foreach (Collider i in enemy1.unitsInRange)
        {
            //check type of unit
            if (i != null && i.GetComponent<Selectable>().unitType.Equals(Selectable.unitTypes.Robot))
            {
                enemyDetected = true;

                //find closest robot
                Vector3 distanceBetwixt = i.transform.position - enemy1.transform.position;
                if (distanceBetwixt.magnitude < closestDistance)
                {
                    enemy1.closestEnemy = i;
                    closestDistance = distanceBetwixt.magnitude;
                }
            }
        }

        //if no enemy is detected set closest enemy to null
        if (!enemyDetected)
        {
            enemy1.closestEnemy = null;
        }
    }
}
