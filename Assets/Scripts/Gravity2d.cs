using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity2d : MonoBehaviour
{
    public float range = 30f;

    public float G = 0.01f;//6.67408f * 0.00000000001f;
    Rigidbody2D ownRb;

    void Awake()
    {
        ownRb = GetComponent<Rigidbody2D>();
        if (ownRb == null)
            ownRb = gameObject.AddComponent<Rigidbody2D>();
        CircleCollider2D trigger = gameObject.AddComponent<CircleCollider2D>();
        trigger.isTrigger = true;
        trigger.radius = range / transform.localScale.x;
    }

    void FixedUpdate()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, range);
        List<Rigidbody2D> rbs = new List<Rigidbody2D>();

        foreach (Collider2D c in cols)
        {
            Rigidbody2D rb = c.attachedRigidbody;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<SpaceShip4Controller2d>().isInGravity = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<SpaceShip4Controller2d>().isInGravity = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
