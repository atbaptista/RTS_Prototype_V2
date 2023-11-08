using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEditor.SearchService;
using UnityEngine.Experimental.AI;
using System.IO.Pipes;

public class George : MonoBehaviour, Moveable
{
    #region header

    [Header("components")]
    public Camera cam;
    public NavMeshAgent playerNavMeshAgent = null;
    public LineRenderer pathLineRenderer;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Selectable selected;

    [Header("variables")]
    [HideInInspector] public Vector3 dest;
    [HideInInspector] public List<Collider> unitsInRange = new List<Collider>();
    [HideInInspector] public Collider closestEnemy = null;
    [HideInInspector] public bool isDestSet = false;
    [HideInInspector] public Queue<Vector3> MoveQueue = new Queue<Vector3>();
    public Material laserMat;
    public float detectionRadius = 5f;
    public float stoppingDistance = 0.15f;
    public float attackSpeed = 0.5f;
    public float basicAttackDmg = 20f;
    // should prob make this health private
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
        // create states
        idleState = new GeorgeIdle(this);
        walkState = new GeorgeWalk(this);
        attackState = new GeorgeAttack(this);
        dieState = new GeorgeDie(this);
        aMoveTargetState = new GeorgeAMoveTarget(this);

        // get components
        anim = GetComponent<Animator>();
        selected = GetComponent<Selectable>();
        selected.unitType = Selectable.unitTypes.Robot;
        selected.health = health;

        // draw green circle 
        Color color = new Color(0, 255, 0);
        selected.DrawCircle(this.gameObject, 1.2f, 0.09f, color);

        // set dest to current position so when pressing shift and showing the path it doesnt point to origin
        dest = this.transform.position;

        // start in idle state
        georgeMachine.ChangeState(idleState);
    }

    void Update()
    {
        DrawSelectionCircle();
        georgeMachine.Update();
    }


    public void Die()
    {
        Destroy(this.gameObject, deathDeletionTime);
    }

#region Moveable

    public void GoTo()
    {
        dest = MoveQueue.Dequeue();
        isDestSet = true;
        isAMove = false;
    }

    public bool isMovingToDest()
    {
        return isDestSet;
    }

    public void QueueMovement(Vector3 destination)
    {
        MoveQueue.Enqueue(destination);
    }

    public void ClearMoveQueue()
    {
        MoveQueue.Clear();
    }

    public void AMove(RaycastHit hit)
    {
        dest = hit.point;
        //check if input is floor
        if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Selectable"))
        {
            isDestSet = true;
            isAMove = true;
            return;
        }
        //check if dino
        if (hit.collider.GetComponent<Selectable>().unitType == Selectable.unitTypes.Dinosaur)
        {
            isAMove = true;
            isAMoveOnTarget = true;
            closestEnemy = hit.collider;
        }
    }

#endregion

    public void GetEnemiesInRange(ref List<Collider> enemiesList)
    {
        Collider[] newList;

        //clear old list
        enemiesList.Clear();

        //Selectable layermask (7)
        int layerMask = 1 << 7;

        //get all objects near 
        newList = Physics.OverlapSphere(transform.position, detectionRadius, layerMask);

        foreach (Collider i in newList)
        {
            enemiesList.Add(i);
        }
    }

    public void DrawSelectionCircle()
    {
        //enable or disable the selection circle
        if (selected.isSelected)
        {
            var line = selected.GetComponent<LineRenderer>();
            if (selected.health <= 25)
            {
                line.material.color = new Color(255, 0, 0);
            }
            else if (selected.health <= 75)
            {
                line.material.color = new Color(255, 255, 0);
            }
            GetComponent<LineRenderer>().enabled = true;
        }
        else
        {
            GetComponent<LineRenderer>().enabled = false;
        }
    }

    public void DrawMovementPath()
    {
        pathLineRenderer.enabled = true;
        NavMeshPath path = new NavMeshPath();
        List<Vector3> allCorners = new List<Vector3>();

        // get the path from current position to current destination
        NavMesh.CalculatePath(this.transform.position, dest, NavMesh.AllAreas, path);
        allCorners.AddRange(new List<Vector3>(path.corners));

        // get the path from the destination to the next destination and so on
        Vector3 prevLoc = dest;
        foreach (Vector3 loc in MoveQueue)
        {
            NavMesh.CalculatePath(prevLoc, loc, NavMesh.AllAreas, path);
            allCorners.AddRange(new List<Vector3>(path.corners));
            prevLoc = loc;
            path.ClearCorners();
        }

        pathLineRenderer.positionCount = allCorners.Count;
        pathLineRenderer.SetPositions(allCorners.ToArray());
    }

    public void StopDrawMovementPath()
    {
        pathLineRenderer.enabled = false;
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
