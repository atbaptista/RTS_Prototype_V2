using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Walk : IState
{
    Enemy2 enemy2;

    public Enemy2Walk(Enemy2 enemy2)
    {
        this.enemy2 = enemy2;
    }

    public void Enter()
    {
        //enable navagent movement, set destination, animation
        enemy2.enemy1NavMeshAgent.isStopped = false;
        enemy2.enemy1NavMeshAgent.SetDestination(enemy2.dest);
        enemy2.anim.SetBool("isWalking", true);
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }
}
