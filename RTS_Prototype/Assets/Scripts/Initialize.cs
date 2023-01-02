using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Initialize : MonoBehaviour
{
    void Start()
    {
        var navMesh = this.GetComponent<NavMeshAgent>();
        navMesh.avoidancePriority = Random.Range(0,99);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
