using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
{
    public ParticleSystem ExplosionSystem;
    public float explosionRadius = 6;

    //declare states
    [HideInInspector] public Enemy2Idle idleState;
    [HideInInspector] public Enemy2Chase chaseState;
    [HideInInspector] public Enemy2Attack attackState;
    [HideInInspector] public Enemy2Die dieState;
    [HideInInspector] public Enemy2Patrol patrolState;

    public override void Start()
    {
        idleState = new Enemy2Idle(this);
        chaseState = new Enemy2Chase(this);
        attackState = new Enemy2Attack(this);
        dieState = new Enemy2Die(this);
        patrolState = new Enemy2Patrol(this);

        OnStart();

        //set state
        enemy1Machine.ChangeState(idleState);
    }

    public void ExplodeDeath()
    {
        //if i wanna tweak the position of the explosion change loc var
        Vector3 loc = this.transform.position;
        ParticleSystem ps = Instantiate(ExplosionSystem, loc, Quaternion.identity);

        //destroy explosion particle system after it is done playing
        Destroy(ps.gameObject, ps.main.duration);

        Destroy(this.gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.green;
        if (closestEnemy != null)
        {
            Gizmos.DrawLine(transform.position, closestEnemy.transform.position);
        }

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
