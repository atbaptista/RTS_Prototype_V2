using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Die : IState
{
    Enemy2 enemy2;

    public Enemy2Die(Enemy2 enemy2)
    {
        this.enemy2 = enemy2;
    }

    public void Enter()
    {
        //play animation, change layer so it cant be selected
        enemy2.anim.SetTrigger("isDead");
        enemy2.gameObject.layer = 9;

        //change it to dead unit type so georges can switch targets
        enemy2.selected.unitType = Selectable.unitTypes.Dead;

        //stop movement and die
        enemy2.enemy1NavMeshAgent.isStopped = true;
        enemy2.Die();
    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}
