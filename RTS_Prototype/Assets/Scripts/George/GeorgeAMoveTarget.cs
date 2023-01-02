using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeorgeAMoveTarget : IState
{
    George george;
    private float _distanceToEnemy;

    public GeorgeAMoveTarget(George george) {
        this.george = george;
    }

    public void Enter() {
        george.isAMoveOnTarget = false;
        george.isAMove = false;
        george.playerNavMeshAgent.isStopped = false;
/*        george.playerNavMeshAgent.SetDestination(george.dest);*/
        george.anim.SetBool("isWalking", true);
    }

    public void Execute() {
        //check if dead
        if (george.selected.health <= 0) {
            george.georgeMachine.ChangeState(george.dieState);
            return;
        }

        //change to walk state if new destination is input
        if (george.isDestSet) {
            george.georgeMachine.ChangeState(george.walkState);
            return;
        }

        //check if target is dead
        if (george.closestEnemy == null) {
            //a move to the location of the dead dino
            george.georgeMachine.ChangeState(george.walkState);
            return;
        }

        //if in range of enemy attack it
        _distanceToEnemy = (george.transform.position - george.closestEnemy.transform.position).magnitude;
        if (_distanceToEnemy <= george.detectionRadius) {
            george.isAMoveOnTarget = true;
            george.georgeMachine.ChangeState(george.attackState);
        } else {
            george.playerNavMeshAgent.SetDestination(george.closestEnemy.transform.position);
        }
    }

    public void Exit() {
        
        george.playerNavMeshAgent.isStopped = true;
        george.anim.SetBool("isWalking", false);
    }
}
