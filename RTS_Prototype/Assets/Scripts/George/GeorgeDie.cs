using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeorgeDie : IState
{
    George george;

    public GeorgeDie(George george)
    {
        this.george = george;
    }

    public void Enter()
    {
        //play animation, change layer so it cant be selected
        george.anim.SetTrigger("die");
        george.gameObject.layer = 9;

        //change it to dead unit type so georges can switch targets
        george.selected.unitType = Selectable.unitTypes.Dead;

        //stop movement and die
        george.playerNavMeshAgent.isStopped = true;
        george.Die();
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }
}
