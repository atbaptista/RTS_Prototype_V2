using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Attack : IState
{
    Enemy2 enemy2;

    public Enemy2Attack(Enemy2 enemy2)
    {
        this.enemy2 = enemy2;
    }

    public void Enter()
    {
        //stop movement and update animation state
        enemy2.enemy1NavMeshAgent.isStopped = true;
        enemy2.anim.SetInteger("animState", 3);
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
        if (enemy2.selected.health <= 0) //dead
        {
            enemy2.enemy1Machine.ChangeState(enemy2.dieState);
            return;
        }
        else if (enemy2.closestEnemy == null ||
            enemy2.closestEnemy.GetComponent<Selectable>().health <= 0) //enemy is dead/gone
        {
            enemy2.enemy1Machine.ChangeState(enemy2.patrolState);
            return;
        }
    }

    private void Attack()
    {
        Debug.Log("attack2 method called, BOOM!!!!!!");
     
        DamageUnits();

        //spawn explosion effect
        

        //die
        enemy2.ExplodeDeath();
    }

    public void DamageUnits() {
        Collider[] hitList;

        //Selectable layermask (7)
        int layerMask = 1 << 7;

        //get all objects near 
        hitList = Physics.OverlapSphere(enemy2.transform.position, enemy2.explosionRadius, layerMask);

        foreach (Collider i in hitList) {
            i.GetComponent<Selectable>().health -= enemy2.basicAttackDmg;
        }
    }
}
