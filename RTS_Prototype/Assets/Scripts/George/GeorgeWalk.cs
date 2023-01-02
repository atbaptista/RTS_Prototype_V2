using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeorgeWalk : IState
{
    George george;
    private Vector3 prevDest;

    public GeorgeWalk(George george) 
    { 
        this.george = george; 
    }

    public void Enter()
    {
        //enable navagent movement, set destination, animation
        george.playerNavMeshAgent.isStopped = false;
        george.playerNavMeshAgent.SetDestination(george.dest);
        george.isDestSet = true;
        george.anim.SetBool("isWalking", true);
    }

    public void Execute()
    {
        //enable or disable the selection circle
        //george.drawSelectionCircle();

        george.playerNavMeshAgent.SetDestination(george.dest);

        //calculate vector from pos to destination
        Vector3 distanceToDest = george.dest - george.transform.position;

        if (george.selected.health <= 0)
        {
            george.georgeMachine.ChangeState(george.dieState);
            return;
        }

        //if a-move on target
        if (george.isAMoveOnTarget) {
            george.georgeMachine.ChangeState(george.aMoveTargetState);
            return;
        }

        //if a-moved, update closest enemy and change to amovetargetstate if enemy in range
        if (george.isAMove) {
            if (EnemyInRange()) {
                FindClosestEnemy();
                george.georgeMachine.ChangeState(george.aMoveTargetState);
                return;
            }
        }

        //within stopping distance
        if (distanceToDest.magnitude < george.stoppingDistance)
        {
            george.georgeMachine.ChangeState(george.idleState);
        }
    }

    public void Exit()
    {
        george.isAMove = false;
        george.isDestSet = false;
        george.playerNavMeshAgent.isStopped = true;
        george.anim.SetBool("isWalking", false);
    }

    private bool EnemyInRange() {
        //update the unitsInRange list
        george.getEnemiesInRange(george.unitsInRange);

        //change state to attack if enemies are detected
        foreach (Collider i in george.unitsInRange) {
            if (i.GetComponent<Selectable>().unitType.Equals
                (Selectable.unitTypes.Dinosaur)) {
                return true;
            }
        }
        return false;
    }

    private void FindClosestEnemy() {
        //detection radius is the max distance objects will be, add 10 to get edge cases
        float closestDistance = george.detectionRadius + 10f;

        foreach (Collider i in george.unitsInRange) {
            //check type of unit
            if (i != null && i.GetComponent<Selectable>().unitType.Equals(Selectable.unitTypes.Dinosaur)) {
                //find closest dinosaur
                Vector3 distanceBetwixt = i.transform.position - george.transform.position;
                if (distanceBetwixt.magnitude < closestDistance) {
                    george.closestEnemy = i;
                    closestDistance = distanceBetwixt.magnitude;
                }
            }
        }
    }
}
