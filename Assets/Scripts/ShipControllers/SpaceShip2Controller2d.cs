using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Transform))]

public class SpaceShip2Controller2d : MonoBehaviour
{
    public static event Action EscEvent;    

    public float moveForce = 1;
    public float rotationForce = .5f;
    public float angularDeceleration= .5f;
    public float moveDeceleration = 1;
    public float shootForce = 1;

    //public ParticleSystem mainNozzle;
    public ParticleSystem downNozzle1;
    public ParticleSystem downNozzle2;
    public ParticleSystem upNozzle1;
    public ParticleSystem upNozzle2;

    public GameObject dockingMethodPrefab;

    [HideInInspector] public bool isInGravity = false;
    
	//private Rope rope;
    private IDockingMethod dockingMethod;

    private Rigidbody2D _rigidBody;
    private Transform _transform;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _transform = transform;
        //if (mainNozzle == null)
        //    mainNozzle = _transform.FindChild("Nozzle").GetComponent<ParticleSystem>();
    }

    private void LateUpdate()
    {
        Vector3 force = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            force = _transform.up * moveForce;
            force = new Vector3(force.x, force.y, 0);
            Move(force, _transform.position);
            //mainNozzle.Play();
            downNozzle1.Play();
            downNozzle2.Play();
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            force = -_transform.up * moveForce;
            force = new Vector3(force.x, force.y, 0);
            Move(force, _transform.position);
            //mainNozzle.Play();
            upNozzle1.Play();
            upNozzle2.Play();
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //force = _transform.up * rotationForce;
            _rigidBody.AddTorque(rotationForce, ForceMode2D.Force);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            //force = -_transform.up * rotationForce;
            _rigidBody.AddTorque(-rotationForce, ForceMode2D.Force);
        }

        //For rope
        //if (Input.GetKeyDown(KeyCode.Q) && rope != null)
        //{
        //    rope.DestroyRope(_transform.up);
        //    rope = null;
        //}
        //if (Input.GetKeyDown(KeyCode.E) && rope == null)
        //{
        //    print("shoot");
        //    GameObject ropeObj = Instantiate(ropePrefab, _transform);
        //    rope = ropeObj.GetComponent<Rope>();
        //    rope.Setup(_rigidBody, -_transform.up * shootForce);
        //}
        //For gravity ray
        if (Input.GetKeyDown(KeyCode.Q) && dockingMethod != null)
        {
            dockingMethod.DestroyObject();
            dockingMethod = null;
        }
        if (Input.GetKeyDown(KeyCode.E) && dockingMethod == null)
        {
            GameObject dockingMethodObj = Instantiate(dockingMethodPrefab, _transform);
            dockingMethod = dockingMethodObj.GetComponent<IDockingMethod>();
            dockingMethod.Setup(gameObject, dockingMethodPrefab.transform.localPosition);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            EscEvent();
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {            
            //mainNozzle.Stop();
            downNozzle1.Stop();
            downNozzle2.Stop();
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            upNozzle1.Stop();
            upNozzle2.Stop();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            downNozzle1.Stop();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            downNozzle2.Stop();
        }
        //Move deceleration
        if (!isInGravity && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKeyDown(KeyCode.UpArrow) && !Input.GetKeyUp(KeyCode.UpArrow) && _rigidBody.velocity.magnitude > 0)
        {
            if (_rigidBody.velocity.magnitude < 0.1f)
            {
                _rigidBody.velocity = Vector2.zero;
                AngularDeveleration();
                return;
            }
            force = -_rigidBody.velocity.normalized * moveDeceleration;
            force = new Vector3(force.x, force.y, 0);
            Move(force, _transform.position);
        }
        else if (!isInGravity && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKeyDown(KeyCode.DownArrow) && !Input.GetKeyUp(KeyCode.DownArrow) && _rigidBody.velocity.magnitude > 0)
        {
            if (_rigidBody.velocity.magnitude < 0.1f)
            {
                _rigidBody.velocity = Vector2.zero;
                AngularDeveleration();
                return;
            }
            force = -_rigidBody.velocity.normalized * moveDeceleration;
            force = new Vector3(force.x, force.y, 0);
            Move(force, _transform.position);
        }
        //Rotate deceleration
        AngularDeveleration();
    }

    private void AngularDeveleration()
    {
        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyUp(KeyCode.LeftArrow) && _rigidBody.angularVelocity > 0)
        {
            if (_rigidBody.angularVelocity < 1)
            {
                _rigidBody.angularVelocity = 0;
                return;
            }
            _rigidBody.AddTorque(-angularDeceleration, ForceMode2D.Force);
        }
        else if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKeyUp(KeyCode.RightArrow) && _rigidBody.angularVelocity < 0)
        {
            if (_rigidBody.angularVelocity > -1)
            {
                _rigidBody.angularVelocity = 0;
                return;
            }
            _rigidBody.AddTorque(angularDeceleration, ForceMode2D.Force);
        }
    }

    private void Move(Vector2 force, Vector2 position)
    {
        _rigidBody.AddForceAtPosition(force, position, ForceMode2D.Force);
    }
}
