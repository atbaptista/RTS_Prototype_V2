using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Patrol : IState
{
    Enemy1 enemy1;

    private float within = 0.3f;

    public Enemy1Patrol(Enemy1 enemy1)
    {
        this.enemy1 = enemy1;
    }

    public void Enter()
    {
        enemy1.anim.SetInteger("animState", 1);
        enemy1.enemy1NavMeshAgent.isStopped = false;
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
        if (enemy1.selected.health <= 0) //dead
        {
            enemy1.enemy1Machine.ChangeState(enemy1.dieState);
        }
        else if (enemiesDetected()) //chase
        {
            enemy1.enemy1Machine.ChangeState(enemy1.chaseState);
        }
        else if (enemy1.patrolEnd == null) //if patrol isnt set
        {
            //if not at starting pos
            if (!isWithin(enemy1.transform.position, enemy1.patrolStart.transform.position, within))
            {
                //go to starting position
                enemy1.enemy1NavMeshAgent.SetDestination(enemy1.patrolStart.transform.position);
            }
            else //idle when arrive
            {
                enemy1.enemy1Machine.ChangeState(enemy1.idleState);
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
        if (enemy1.going && !isWithin(enemy1.transform.position, enemy1.patrolEnd.transform.position, within))
        {
            //continue going to patrolEnd
            enemy1.enemy1NavMeshAgent.SetDestination(enemy1.patrolEnd.transform.position);
        }
        //reached patrol end
        else if (enemy1.going && isWithin(enemy1.transform.position, enemy1.patrolEnd.transform.position, within))
        {
            //go back to patrol start, set going to false
            enemy1.enemy1NavMeshAgent.SetDestination(enemy1.patrolStart.transform.position);
            enemy1.going = false;
        }
        //going to patrol start and not at patrol start yet
        else if (!enemy1.going && !isWithin(enemy1.transform.position, enemy1.patrolStart.transform.position, within))
        {
            //go to patrol start
            enemy1.enemy1NavMeshAgent.SetDestination(enemy1.patrolStart.transform.position);
        }
        //going to patrol start and at patrol start
        else
        {
            //go to patrol end
            enemy1.enemy1NavMeshAgent.SetDestination(enemy1.patrolEnd.transform.position);
            enemy1.going = true;
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
