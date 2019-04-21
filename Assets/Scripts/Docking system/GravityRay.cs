using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityRay : MonoBehaviour
{
    public Transform pointsTransform;
    public float velocity = 3;
    public float G = 0.01f;

    private GameObject cargo;
    private Rigidbody2D cargoRigidbody;
    private Rigidbody2D shipRigidbody;
    private Transform[] points;
    private Transform ship;
    private Transform _transform;

    private void Awake()
    {
        points = pointsTransform.GetComponentsInChildren<Transform>();
        _transform = transform;
    }

    public void Setup(Transform parent, Vector3 position, Vector3 scale)
    {
        ship = parent;
        shipRigidbody = ship.GetComponent<Rigidbody2D>();
        _transform.localRotation = Quaternion.identity;
        _transform.localPosition = position;
        _transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Pickable")
        {
            print("pickable");
            cargo = collision.gameObject;
            //cargo.transform.parent = ship;
            cargoRigidbody = cargo.GetComponent<Rigidbody2D>();
            StartCoroutine("Docking");
        }
    }

    private IEnumerator Docking()
    {
        //cargoRigidbody.velocity = offset.normalized * velocity;
        //cargoRigidbody.velocity = ship.GetComponent<Rigidbody2D>().velocity;
        while (true)
        {
            Vector3 offset = _transform.position - cargo.transform.position;
            Vector3 gravity = offset.normalized * shipRigidbody.mass * cargoRigidbody.mass * G / offset.sqrMagnitude;
            if (offset.magnitude >= .3f)
                cargoRigidbody.AddForce(gravity);
            yield return null;
        }
        //for(int i=0; i<=points.Length-1;i++)
        //{
        //    while(cargo.transform.localPosition.y <= points[i].localPosition.y)
        //    {
        //        cargoRigidbody.velocity = (points[i].position - cargo.transform.position).normalized * velocity;
        //        cargoRigidbody.angularVelocity = velocity;
        //        yield return new WaitForSeconds(.3f);
        //    }
        //}
        //cargoRigidbody.velocity = Vector2.zero;
    }

    public void DestroyRay()
    {
        StopCoroutine("Docking");
        Destroy(gameObject);
    }
}
