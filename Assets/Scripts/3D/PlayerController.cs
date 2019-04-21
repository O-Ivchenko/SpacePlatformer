using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Transform))]

public class PlayerController : MonoBehaviour
{
    public float moveForce = 1;
    public float moveTorque = 1;
    public ParticleSystem nozzle;

    private Rigidbody _rigidBody;
    private Transform _transform;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _transform = transform;
        if (nozzle == null)
            nozzle = _transform.Find("Nozzle").GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        Vector3 force = Vector3.zero;
        Vector3 torque = Vector3.zero;
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            force = _transform.up * moveForce;
            force = new Vector3(force.x, force.y, 0);
            torque = Vector3.zero;
            Move(force, torque);
            nozzle.Play();
            return;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            force = Vector3.zero;
            torque = new Vector3(0, 0, moveTorque);
            Move(force, torque);
            nozzle.Stop();
            return;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            force = Vector3.zero;
            torque = new Vector3(0, 0, -moveTorque);
            Move(force, torque);
            nozzle.Stop();
            return;
        }
        else
            nozzle.Stop();
        //else if(Input.GetKey(KeyCode.DownArrow))
        //{
        //    force = -_transform.up * moveForce;
        //    force = new Vector3(force.x, force.y, 0);
        //    torque = Vector3.zero;
        //    Move(force, torque);
        //    return;
        //}
    }

    private void Move(Vector3 force, Vector3 torque)
    {
        _rigidBody.AddForce(force, ForceMode.Force);
        _rigidBody.AddTorque(torque, ForceMode.Force);
    }

    //void OnDrawGizmos()
    //{
    //    Color color;
    //    //color = Color.green;
    //    //// local up
    //    //DrawHelperAtCenter(this.transform.up, color, 2f);

    //    //color.g -= 0.5f;
    //    //// global up
    //    //DrawHelperAtCenter(Vector3.up, color, 1f);

    //    //color = Color.blue;
    //    //// local forward
    //    //DrawHelperAtCenter(this.transform.forward, color, 2f);

    //    //color.b -= 0.5f;
    //    //// global forward
    //    //DrawHelperAtCenter(Vector3.forward, color, 1f);

    //    //color = Color.red;
    //    //// local right
    //    //DrawHelperAtCenter(this.transform.right, color, 2f);

    //    //color.r -= 0.5f;
    //    //// global right
    //    //DrawHelperAtCenter(Vector3.right, color, 1f);

    //    color = Color.white;
    //    DrawHelperAtCenter(new Vector3(transform.up.x * moveForce, transform.up.y * moveForce, 0), color, 2f);
    //}

    //private void DrawHelperAtCenter(
    //                   Vector3 direction, Color color, float scale)
    //{
    //    Gizmos.color = color;
    //    Vector3 destination = transform.position + direction * scale;
    //    Gizmos.DrawLine(transform.position, destination);
    //}
}
