using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class George : MonoBehaviour, Moveable 
{
    #region header

    //components
    [Header("components")]
    public Camera cam;
    public NavMeshAgent playerNavMeshAgent = null;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Selectable selected;

    //variables
    [Header("variables")]
    [HideInInspector] public Vector3 dest;
    [HideInInspector] public List<Collider> unitsInRange = new List<Collider>();
    [HideInInspector] public Collider closestEnemy = null;
    [HideInInspector] public bool isDestSet = false;
    public Material laserMat;
    public float detectionRadius = 5f;
    public float stoppingDistance = 0.15f;
    public float attackSpeed = 0.5f;
    public float basicAttackDmg = 20f;
    //should prob make this health private
    public float health = 100f;
    public float deathDeletionTime = 1.5f;

    [Header("debug")]
    public bool isAMove = false;
    public bool isAMoveOnTarget = false;

    //state machine
    [HideInInspector] public StateMachine georgeMachine = new StateMachine();
    [HideInInspector] public GeorgeIdle idleState;
    [HideInInspector] public GeorgeWalk walkState;
    [HideInInspector] public GeorgeAttack attackState;
    [HideInInspector] public GeorgeDie dieState;
    [HideInInspector] public GeorgeAMoveTarget aMoveTargetState;
    #endregion header

    void Start()
    {
        //create states
        idleState = new GeorgeIdle(this);
        walkState = new GeorgeWalk(this);
        attackState = new GeorgeAttack(this);
        dieState = new GeorgeDie(this);
        aMoveTargetState = new GeorgeAMoveTarget(this);

        //get components
        anim = GetComponent<Animator>();
        selected = GetComponent<Selectable>();
        selected.unitType = Selectable.unitTypes.Robot;
        selected.health = health;

        //draw green circle, start in idle state
        Color color = new Color(0, 255, 0);
        selected.DrawCircle(this.gameObject, 1.2f, 0.09f, color);
        georgeMachine.ChangeState(idleState);
    }

    void Update()
    {
        drawSelectionCircle();
        georgeMachine.Update();
    }


    public void Die()
    {
        Destroy(this.gameObject, deathDeletionTime);
    }

    public void GoTo(Vector3 destination)
    {
        dest = destination;
        isDestSet = true;
        isAMove = false;
    }

    public void AMove(RaycastHit hit) {
        dest = hit.point;
        //check if input is floor
        if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Selectable")) {
            isDestSet = true;
            isAMove = true;
            return;
        }
        //check if dino
        if (hit.collider.GetComponent<Selectable>().unitType == Selectable.unitTypes.Dinosaur) {
            isAMove = true;
            isAMoveOnTarget = true;
            closestEnemy = hit.collider;
        }
    }

    public void getEnemiesInRange(List<Collider> enemiesList)
    {
        Collider[] newList;

        //clear old list
        enemiesList.Clear();

        //Selectable layermask (7)
        int layerMask = 1 << 7;

        //get all objects near 
        newList = Physics.OverlapSphere(transform.position, detectionRadius, layerMask);

        foreach(Collider i in newList)
        {
            enemiesList.Add(i);
        }
    }

    public void drawSelectionCircle()
    {
        //enable or disable the selection circle
        if (selected.isSelected)
        {
            var line = selected.GetComponent<LineRenderer>();
            if (selected.health <= 50)
            {
                line.material.color = new Color(255, 0, 0);
            }
            GetComponent<LineRenderer>().enabled = true;
        }
        else
        {
            GetComponent<LineRenderer>().enabled = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.green;
        if (closestEnemy != null)
        {
            Gizmos.DrawLine(transform.position, closestEnemy.transform.position);
        }
    }
}
