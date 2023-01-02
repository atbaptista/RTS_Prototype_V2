using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Idle : IState
{
    Enemy2 enemy2;

    public Enemy2Idle(Enemy2 enemy2)
    {
        this.enemy2 = enemy2;
    }

    public void Enter()
    {
        //update navmesh, animation
        enemy2.enemy1NavMeshAgent.isStopped = true;
        enemy2.anim.SetInteger("animState", 0);
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
        if (enemy2.selected.health <= 0) //dead
        {
            enemy2.enemy1Machine.ChangeState(enemy2.dieState);
        }
        else if (enemiesDetected()) //chase
        {
            enemy2.enemy1Machine.ChangeState(enemy2.chaseState);
        }
        else if (enemy2.patrolEnd != null) //patrol
        {
            enemy2.enemy1Machine.ChangeState(enemy2.patrolState);
        }
    }

    private bool enemiesDetected()
    {
        //update units in range list
        enemy2.getUnitsInRange(enemy2.unitsInRange);

        //return true if any robots are detected
        foreach (Collider i in enemy2.unitsInRange)
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
