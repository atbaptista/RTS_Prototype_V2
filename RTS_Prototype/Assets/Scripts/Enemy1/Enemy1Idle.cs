using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Idle : IState
{
    Enemy1 enemy1;

    public Enemy1Idle(Enemy1 enemy1)
    {
        this.enemy1 = enemy1;
    }

    public void Enter()
    {
        //update navmesh, animation
        enemy1.enemy1NavMeshAgent.isStopped = true;
        enemy1.anim.SetInteger("animState", 0);
    }

    public void Execute()
    {
        decideState();
    }

    public void Exit()
    {

    }

    private void decideState()
    {
        if (enemy1.selected.health <= 0) //dead
        {
            enemy1.enemy1Machine.ChangeState(enemy1.dieState);
        }
        else if (enemiesDetected()) //chase
        {
            enemy1.enemy1Machine.ChangeState(enemy1.chaseState);
        }
        else if (enemy1.patrolEnd != null) //patrol
        {
            enemy1.enemy1Machine.ChangeState(enemy1.patrolState);
        }
    }

    private bool enemiesDetected()
    {
        //update units in range list
        enemy1.getUnitsInRange(enemy1.unitsInRange);

        //return true if any robots are detected
        foreach (Collider i in enemy1.unitsInRange)
        {
            if (i.GetComponent<Selectable>().unitType.Equals
                (Selectable.unitTypes.Robot))
            {
                return true;
            }
        }

        //no robots detected :(
        return false;
    }
}
