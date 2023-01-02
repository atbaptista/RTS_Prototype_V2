using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Attack : IState
{
    Enemy1 enemy1;
    private float attackTime;

    public Enemy1Attack(Enemy1 enemy1)
    {
        this.enemy1 = enemy1;
    }

    public void Enter()
    {
        //set when to attack
        attackTime = Time.time + enemy1.attackSpeed;

        //stop movement and update animation state
        enemy1.enemy1NavMeshAgent.isStopped = true;
        enemy1.anim.SetInteger("animState", 3);
    }

    public void Execute()
    {
        decideState();

        Attack();
    }

    public void Exit()
    {
        
    }



    private void decideState()
    {
        if (enemy1.selected.health <= 0) //dead
        {
            enemy1.enemy1Machine.ChangeState(enemy1.dieState);
            return;
        }
        else if (enemy1.closestEnemy == null ||
            enemy1.closestEnemy.GetComponent<Selectable>().health <= 0) //enemy is dead/gone
        {
            enemy1.enemy1Machine.ChangeState(enemy1.patrolState);
            return;
        }
    }

    private void Attack()
    {
        Debug.Log("attack method called");
        if (Time.time > attackTime)
        {
            Debug.Log("chomp");
            enemy1.closestEnemy.GetComponent<Selectable>().health -= enemy1.basicAttackDmg;
            enemy1.enemy1Machine.ChangeState(enemy1.chaseState);
        }
    }
}
