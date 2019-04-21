using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Transform))]

public class SpaceShipController : MonoBehaviour
{
    public float moveForce = 1;
    public float rotationForce = .5f;
    public ParticleSystem mainNozzle;
    public ParticleSystem UpNozzle1;
    public ParticleSystem UpNozzle2;
    public ParticleSystem downNozzle1;
    public ParticleSystem downNozzle2;
    public Transform DockingPoint;

    private Rigidbody _rigidBody;
    private Transform _transform;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _transform = transform;
        if (mainNozzle == null)
            mainNozzle = _transform.Find("Nozzle").GetComponent<ParticleSystem>();
    }

    private void LateUpdate()
    {
        Vector3 force = Vector3.zero;

        if (Input.GetKey(KeyCode.Space))
        {
            force = _transform.right * moveForce;
            force = new Vector3(force.x, force.y, 0);
            
            Move(force, mainNozzle.transform.position);
            mainNozzle.Play();
            //return;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            force = _transform.up * rotationForce;
            force = new Vector3(force.x, force.y, 0);

            Move(force, downNozzle1.transform.position);
            downNozzle1.Play();

            Move(-force, UpNozzle2.transform.position);
            UpNozzle2.Play();

            //return;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            force = -_transform.up * rotationForce;
            force = new Vector3(force.x, force.y, 0);

            Move(force, UpNozzle1.transform.position);
            UpNozzle1.Play();

            Move(-force, downNozzle2.transform.position);
            downNozzle2.Play();

            //return;
        }

        if(Input.GetKey(KeyCode.UpArrow))
        {
            force = _transform.up * rotationForce;
            force = new Vector3(force.x, force.y, 0);

            Move(force, _transform.position);
            downNozzle1.Play();

            //Move(force, downNozzle2.transform.position);
            downNozzle2.Play();
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            force = -_transform.up * rotationForce;
            force = new Vector3(force.x, force.y, 0);

            Move(force, _transform.position);
            UpNozzle1.Play();

            //Move(force, UpNozzle2.transform.position);
            UpNozzle2.Play();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {            
            mainNozzle.Stop();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            UpNozzle2.Stop();
            downNozzle1.Stop();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            UpNozzle1.Stop();
            downNozzle2.Stop();
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            downNozzle1.Stop();
            downNozzle2.Stop();
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            UpNozzle1.Stop();
            UpNozzle2.Stop();
        }

        if (Input.GetKeyDown(KeyCode.E))
            CastBox(DockingPoint.position, Vector3.one, -_transform.up);
    }

    private void CastBox(Vector3 center, Vector3 halfSize, Vector3 direction)
    {
        print("cast box " + center);
        RaycastHit hitInfo;
        if(Physics.BoxCast(center, halfSize, direction, out hitInfo, Quaternion.identity, halfSize.x * 2))
        {
            if (hitInfo.collider.tag == "Pickable")
                Debug.Log(hitInfo.collider.name);
        }
    }

    private void Move(Vector3 force, Vector3 position)
    {
        _rigidBody.AddForceAtPosition(force, position, ForceMode.Force);
        //_rigidBody.AddTorque(torque, ForceMode.Force);
    }
}
