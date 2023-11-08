using UnityEngine;

public class Enemy1 : Enemy
{
    // declare states
    [HideInInspector] public Enemy1Idle idleState;
    [HideInInspector] public Enemy1Chase chaseState;
    [HideInInspector] public Enemy1Attack attackState;
    [HideInInspector] public Enemy1Die dieState;
    [HideInInspector] public Enemy1Patrol patrolState;

    public override void Start()
    {
        idleState = new Enemy1Idle(this);
        chaseState = new Enemy1Chase(this);
        attackState = new Enemy1Attack(this);
        dieState = new Enemy1Die(this);
        patrolState = new Enemy1Patrol(this);

        // initialize and get stuff ready
        OnStart();

        // set state
        enemy1Machine.ChangeState(idleState);
    }

}
