using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Patrol : IState
{
    Enemy2 enemy2;

    private float within = 0.3f;

    public Enemy2Patrol(Enemy2 enemy2)
    {
        this.enemy2 = enemy2;
    }

    public void Enter()
    {
        enemy2.anim.SetInteger("animState", 1);
        enemy2.enemy1NavMeshAgent.isStopped = false;
    }

    public void Execute()
    {
        DecideAction();
    }

    public void Exit()
    {

    }



    private void DecideAction()
    {
        if (enemy2.selected.health <= 0) //dead
        {
            enemy2.enemy1Machine.ChangeState(enemy2.dieState);
        }
        else if (enemiesDetected()) //chase
        {
            enemy2.enemy1Machine.ChangeState(enemy2.chaseState);
        }
        else if (enemy2.patrolEnd == null) //if patrol isnt set
        {
            //if not at starting pos
            if (!isWithin(enemy2.transform.position, enemy2.patrolStart.transform.position, within))
            {
                //go to starting position
                enemy2.enemy1NavMeshAgent.SetDestination(enemy2.patrolStart.transform.position);
            }
            else //idle when arrive
            {
                enemy2.enemy1Machine.ChangeState(enemy2.idleState);
            }
        }
        else //patrol
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        //going to patrol end and not there yet
        if (enemy2.going && !isWithin(enemy2.transform.position, enemy2.patrolEnd.transform.position, within))
        {
            //continue going to patrolEnd
            enemy2.enemy1NavMeshAgent.SetDestination(enemy2.patrolEnd.transform.position);
        }
        //reached patrol end
        else if (enemy2.going && isWithin(enemy2.transform.position, enemy2.patrolEnd.transform.position, within))
        {
            //go back to patrol start, set going to false
            enemy2.enemy1NavMeshAgent.SetDestination(enemy2.patrolStart.transform.position);
            enemy2.going = false;
        }
        //going to patrol start and not at patrol start yet
        else if (!enemy2.going && !isWithin(enemy2.transform.position, enemy2.patrolStart.transform.position, within))
        {
            //go to patrol start
            enemy2.enemy1NavMeshAgent.SetDestination(enemy2.patrolStart.transform.position);
        }
        //going to patrol start and at patrol start
        else
        {
            //go to patrol end
            enemy2.enemy1NavMeshAgent.SetDestination(enemy2.patrolEnd.transform.position);
            enemy2.going = true;
        }
    }

    private bool isWithin(Vector3 thisPos, Vector3 thatPos, float within)
    {
        Vector3 distanceBetwixt = thisPos - thatPos;
        if (distanceBetwixt.magnitude < within)
        {
            return true;
        }
        return false;
    }

    private bool enemiesDetected()
    {
        //update units in range list
        enemy2.GetUnitsInRange(enemy2.unitsInRange);

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
