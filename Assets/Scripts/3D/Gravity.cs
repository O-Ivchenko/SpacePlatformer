using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float range = 30f;

    public float G = 0.001f;//6.67408f * 0.00000000001f;
    Rigidbody ownRb;

    void Awake()
    {
        ownRb = GetComponent<Rigidbody>();
        if (ownRb == null)
            ownRb = gameObject.AddComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, range);
        List<Rigidbody> rbs = new List<Rigidbody>();

        foreach (Collider c in cols)
        {
            Rigidbody rb = c.attachedRigidbody;
            if (rb != null && rb != ownRb && !rbs.Contains(rb))
            {
                rbs.Add(rb);
                Vector3 offset = transform.position - c.transform.position;
                Vector3 gravity = offset.normalized * ownRb.mass * rb.mass * G / offset.sqrMagnitude;
                rb.AddForce(gravity);
                //print("add force " + rb.name + " " + gravity);
            }
        }
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.cyan;
    //    Gizmos.DrawWireSphere(transform.position, range);
    //}
}
