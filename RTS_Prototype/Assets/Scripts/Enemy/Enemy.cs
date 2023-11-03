using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{

#region header

    // components
    public NavMeshAgent enemy1NavMeshAgent = null;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Selectable selected;

    // variables
    [HideInInspector] public Vector3 dest;
    [HideInInspector] public List<Collider> unitsInRange = new List<Collider>();
    [HideInInspector] public Collider closestEnemy = null;
    [HideInInspector] public bool going = true;
    public GameObject patrolStart;
    public GameObject patrolEnd = null;
    public float deathDeletionTime = 1.2f;
    public float attackRadius = 2f;
    public float detectionRadius = 5f;
    public float stoppingDistance = 0.15f;
    public float basicAttackDmg = 40f;
    public float attackSpeed = 1f;
    public float chaseSpeedIncrease = 5f;
    [SerializeField] private float health = 100f;



    // state machine
    [HideInInspector] public StateMachine enemy1Machine = new StateMachine();
    /*    [HideInInspector] public IState idleState;
        [HideInInspector] public IState chaseState;
        [HideInInspector] public IState attackState;
        [HideInInspector] public IState dieState;
        [HideInInspector] public IState patrolState;*/

#endregion header

    public abstract void Start();

    public void OnStart()
    {
        // get components and initialize stuff
        anim = GetComponent<Animator>();
        selected = GetComponent<Selectable>();
        selected.unitType = Selectable.unitTypes.Dinosaur;
        selected.health = health;
        if (patrolEnd == null)
        {
            going = false;
        }

        // make an empty gameobject and set its location to where the dino spawns
        patrolStart = new GameObject("patrolStart for " + name);
        patrolStart.transform.position = transform.position;

        // draw red circle, disable it until selection is decided upon
        Color color = new Color(255, 0, 0);
        selected.DrawCircle(this.gameObject, 1.2f, 0.09f, color);
        GetComponent<LineRenderer>().enabled = false;
    }

    void Update()
    {
        enemy1Machine.Update();
    }

    // can put these two methods within the selectable class
    public void Die()
    {
        Destroy(this.gameObject, deathDeletionTime);
    }

    public void GetUnitsInRange(List<Collider> unitsList)
    {
        Collider[] newList;

        // clear old list
        unitsList.Clear();

        // Selectable layermask (7)
        int layerMask = 1 << 7;

        // get all objects near 
        newList = Physics.OverlapSphere(transform.position, detectionRadius, layerMask);

        foreach (Collider i in newList)
        {
            unitsList.Add(i);
        }
    }
    public void DrawSelectionCircle()
    {
        // enable or disable the selection circle
        if (selected.isSelected)
        {
            GetComponent<LineRenderer>().enabled = true;
        }
        else
        {
            GetComponent<LineRenderer>().enabled = false;
        }
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
    }
}
