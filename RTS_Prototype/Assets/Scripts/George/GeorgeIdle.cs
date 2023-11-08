using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeorgeIdle : IState
{
    George george;

    public GeorgeIdle(in George george)
    {
        this.george = george;
    }

    public void Enter(){}
    public void Exit(){}

    public void Execute()
    {
        ChooseAction();
    }

    private void ChooseAction()
    {
        if (george.selected.health <= 0)
        {
            george.georgeMachine.ChangeState(george.dieState);
        }
        // check if new destination is set
        else if (george.isDestSet) 
        {
            george.georgeMachine.ChangeState(george.walkState);
        }
        // check if the attack move selected a target
        else if (george.isAMoveOnTarget)
        { 
            george.georgeMachine.ChangeState(george.aMoveTargetState);
        }
        // check if enemies are near
        else 
        {
            // update the unitsInRange list
            george.GetEnemiesInRange(ref george.unitsInRange);

            // change state to attack if enemies are detected
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