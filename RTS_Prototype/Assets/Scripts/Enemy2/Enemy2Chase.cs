using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Chase : IState
{
    Enemy2 enemy2;

    public Enemy2Chase(Enemy2 enemy2)
    {
        this.enemy2 = enemy2;
    }

    public void Enter()
    {
        enemy2.enemy1NavMeshAgent.isStopped = false;
        enemy2.anim.SetInteger("animState", 2);
        enemy2.enemy1NavMeshAgent.speed += enemy2.chaseSpeedIncrease;
    }

    public void Execute()
    {
        FindClosestEnemy();

        DecideAction();
    }

    public void Exit()
    {
        enemy2.enemy1NavMeshAgent.speed -= enemy2.chaseSpeedIncrease;
    }





    private void DecideAction()
    {
        Vector3 distanceBetwixt = Vector3.zero;

        if (enemy2.closestEnemy != null)
        {
            distanceBetwixt = enemy2.closestEnemy.transform.position - enemy2.transform.position;
        }

        if (enemy2.selected.health <= 0) //dead
        {
            enemy2.enemy1Machine.ChangeState(enemy2.dieState);
            return;
        }
        else if (enemy2.closestEnemy == null) //no units in range 
        {
            enemy2.enemy1Machine.ChangeState(enemy2.patrolState);
            return;
        }
        else if (distanceBetwixt.magnitude <= enemy2.attackRadius) //unit in attackable range
        {
            enemy2.enemy1Machine.ChangeState(enemy2.attackState);
            return;
        }
        else //chase enemy
        {
            enemy2.enemy1NavMeshAgent.SetDestination(enemy2.closestEnemy.transform.position);
        }
    }

    private void FindClosestEnemy()
    {
        //update enemies near list
        enemy2.GetUnitsInRange(enemy2.unitsInRange);

        //detection radius is the max distance objects will be, add 10 to get edge cases
        float closestDistance = enemy2.detectionRadius + 10f;

        bool enemyDetected = false;

        foreach (Collider i in enemy2.unitsInRange)
        {
            //check type of unit
            if (i != null && i.GetComponent<Selectable>().unitType.Equals(Selectable.unitTypes.Robot))
            {
                enemyDetected = true;

                //find closest robot
                Vector3 distanceBetwixt = i.transform.position - enemy2.transform.position;
                if (distanceBetwixt.magnitude < closestDistance)
                {
                    enemy2.closestEnemy = i;
                    closestDistance = distanceBetwixt.magnitude;
                }
            }
        }

        //if no enemy is detected set closest enemy to null
        if (!enemyDetected)
        {
            enemy2.closestEnemy = null;
        }
    }
}
