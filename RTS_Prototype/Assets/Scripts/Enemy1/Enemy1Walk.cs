using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Walk : IState
{
    Enemy1 enemy1;

    public Enemy1Walk(Enemy1 enemy1)
    {
        this.enemy1 = enemy1;
    }

    public void Enter()
    {
        //enable navagent movement, set destination, animation
        enemy1.enemy1NavMeshAgent.isStopped = false;
        enemy1.enemy1NavMeshAgent.SetDestination(enemy1.dest);
        enemy1.anim.SetBool("isWalking", true);
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }
}
