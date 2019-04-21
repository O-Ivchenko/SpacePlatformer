using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Transform))]

public class SpaceShip3Controller2d : MonoBehaviour
{
    public float moveForce = 1;
    public Vector3 rotateAngle = new Vector3(0,0,1);

    public ParticleSystem mainNozzle;
    public ParticleSystem downNozzle1;
    public ParticleSystem downNozzle2;

    public Transform DockingPoint;

    private Rigidbody2D _rigidBody;
    private Transform _transform;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _transform = transform;
        if (mainNozzle == null)
            mainNozzle = _transform.Find("Nozzle").GetComponent<ParticleSystem>();
    }

    private void LateUpdate()
    {
        Vector3 force = Vector3.zero;

        if (Input.GetKey(KeyCode.Space))
        {
            force = _transform.up * moveForce;
            force = new Vector3(force.x, force.y, 0);
            //Vector2 position = new Vector2(mainNozzle.transform.position.x, 0);
            Move(force, _transform.position);
            mainNozzle.Play();
            downNozzle1.Play();
            downNozzle2.Play();
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //force = _transform.up * rotationForce;
            //force = new Vector3(force.x, force.y, 0);

            //Move(force, downNozzle1.transform.position);
            //downNozzle1.Play();
            //_rigidBody.AddTorque(rotationForce, ForceMode2D.Force);
            _transform.Rotate(rotateAngle);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            //force = -_transform.up * rotationForce;
            //force = new Vector3(force.x, force.y, 0);

            //Move(-force, downNozzle2.transform.position);
            //downNozzle2.Play();
            //_rigidBody.AddTorque(-rotationForce, ForceMode2D.Force);
            _transform.Rotate(-rotateAngle);
        }

        //if(Input.GetKey(KeyCode.UpArrow))
        //{
        //    force = _transform.up * rotationForce;
        //    force = new Vector3(force.x, force.y, 0);

        //    Move(force, _transform.position);
        //    downNozzle1.Play();

        //    //Move(force, downNozzle2.transform.position);
        //    downNozzle2.Play();
        //}

        //if (Input.GetKey(KeyCode.DownArrow))
        //{
        //    force = -_transform.up * rotationForce;
        //    force = new Vector3(force.x, force.y, 0);

        //    Move(force, _transform.position);
        //    UpNozzle1.Play();

        //    //Move(force, UpNozzle2.transform.position);
        //    UpNozzle2.Play();
        //}

        if (Input.GetKeyUp(KeyCode.Space))
        {            
            mainNozzle.Stop();
            downNozzle1.Stop();
            downNozzle2.Stop();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            downNozzle1.Stop();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            downNozzle2.Stop();
        }
        //if (Input.GetKeyUp(KeyCode.UpArrow))
        //{
        //    downNozzle1.Stop();
        //    downNozzle2.Stop();
        //}
        //if (Input.GetKeyUp(KeyCode.DownArrow))
        //{
        //    UpNozzle1.Stop();
        //    UpNozzle2.Stop();
        //}

        //if (Input.GetKeyDown(KeyCode.E))
        //    CastBox(DockingPoint.position, Vector3.one, -_transform.up);
    }

    private void CastBox(Vector3 center, Vector3 halfSize, Vector3 direction)
    {
        RaycastHit hitInfo;
        if(Physics.BoxCast(center, halfSize, direction, out hitInfo, Quaternion.identity, halfSize.x * 2))
        {
            if (hitInfo.collider.tag == "Pickable")
                Debug.Log(hitInfo.collider.name);
        }
    }

    private void Move(Vector2 force, Vector2 position)
    {
        _rigidBody.AddForceAtPosition(force, position, ForceMode2D.Force);
        //_rigidBody.AddTorque(torque, ForceMode.Force);
    }
}
