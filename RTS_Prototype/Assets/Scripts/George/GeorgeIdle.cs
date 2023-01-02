using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeorgeIdle : IState
{
    George george;

    public GeorgeIdle(George george) 
    { 
        this.george = george; 
    }

    public void Enter()
    {
        //update navmesh, animation, isdestset
        //george.anim.SetBool("isWalking", false);
        //george.anim.ResetTrigger("doneAttack");
    }

    public void Execute()
    {
        //enable or disable the selection circle
        //george.drawSelectionCircle();

        ChooseAction();
    }

    public void Exit()
    {
        
    }

    private void ChooseAction()
    {
        if (george.selected.health <= 0)
        {
            george.georgeMachine.ChangeState(george.dieState);
        }
        else if (george.isDestSet) //check if new destination is set
        {
            george.georgeMachine.ChangeState(george.walkState);
        }
        else if (george.isAMoveOnTarget) { //check if the attack move selected a target
            george.georgeMachine.ChangeState(george.aMoveTargetState);
        }
        else //check if enemies are near
        {
            //update the unitsInRange list
            george.getEnemiesInRange(george.unitsInRange);

            //change state to attack if enemies are detected
            foreach (Collider i in george.unitsInRange)
            {
                if (i.GetComponent<Selectable>().unitType.Equals
                    (Selectable.unitTypes.Dinosaur))
                {
                    george.georgeMachine.ChangeState(george.attackState);
                }
            }
        }
    }

}