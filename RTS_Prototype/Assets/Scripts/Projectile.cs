using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public GameObject target;
    [HideInInspector] public float projectileSpeed = 10f;
    private Vector3 targetOriginalPos;

    private void Start()
    {
        targetOriginalPos = target.transform.position;

        if (target.Equals(null))
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.Equals(target.transform.position))
        {
            Destroy(this.gameObject);
        }

        transform.position = Vector3.MoveTowards(transform.position, 
            target.transform.position, projectileSpeed * Time.deltaTime);

        //transform.LookAt(target.transform.position);
    }
/*    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }*/
}
