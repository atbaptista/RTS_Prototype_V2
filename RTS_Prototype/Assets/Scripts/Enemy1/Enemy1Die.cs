using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Die : IState
{
    Enemy1 enemy1;

    public Enemy1Die(Enemy1 enemy1)
    {
        this.enemy1 = enemy1;
    }

    public void Enter()
    {
        //play animation, change layer so it cant be selected
        enemy1.anim.SetTrigger("isDead");
        enemy1.gameObject.layer = 9;

        //change it to dead unit type so georges can switch targets
        enemy1.selected.unitType = Selectable.unitTypes.Dead;

        //stop movement and die
        enemy1.enemy1NavMeshAgent.isStopped = true;
        enemy1.Die();
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }
}
