using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Transform))]

public class SpaceShip4Controller2d : MonoBehaviour
{
    //TODO: Remade all events with observer pattern
    public static event Action EscEvent;    

    public float moveForce = 1;
    public float rotationForce = .5f;
    public float angularDeceleration= .5f;
    public float moveDeceleration = 1;
    public float maxAngularVelocity = 20f;
    public float brakeForce = 25f;
    public FuelSystem fuelSystem;

    public GameObject dockingMethodPrefab;

    public Animator engine1;
    public Animator engine2;

    [HideInInspector] public bool isInGravity = false;
    
	//private Rope rope;
    private IDockingMethod dockingMethod;

    private Rigidbody2D _rigidBody;
    private Transform _transform;
    private int angleLimitForAngularDeceleration = 4; // magic number that was selected in practical way

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _transform = transform;
        if (fuelSystem == null)
            fuelSystem = GetComponent<FuelSystem>();
    }

    private void LateUpdate()
    {
        Vector3 force = Vector3.zero;
        float horizontalAxe = InputController.Instance.HorizontalAxe;
        float verticalAxe = InputController.Instance.VerticalAxe;
        float brake = InputController.Instance.Space;

        if (verticalAxe != 0 && fuelSystem.HaveFuel())
        {
            fuelSystem.ConsumeFuel(Mathf.Abs(verticalAxe));
            force = _transform.up * moveForce * verticalAxe;
            force = new Vector3(force.x, force.y, 0);
            Move(force, _transform.position);
        }
        if (horizontalAxe < 0 && _rigidBody.angularVelocity <= maxAngularVelocity)
            _rigidBody.AddTorque(-rotationForce * horizontalAxe, ForceMode2D.Force);
        else if (horizontalAxe > 0 && _rigidBody.angularVelocity >= -maxAngularVelocity)
            _rigidBody.AddTorque(-rotationForce * horizontalAxe, ForceMode2D.Force);

        //For gravity ray
        if (InputController.Instance.KeyE)
        {
            if (dockingMethod == null)
            {
                GameObject dockingMethodObj = Instantiate(dockingMethodPrefab, _transform);
                dockingMethod = dockingMethodObj.GetComponent<IDockingMethod>();
                dockingMethod.Setup(gameObject, dockingMethodPrefab.transform.localPosition);
            }
            else
            {
                dockingMethod.DestroyObject();
                dockingMethod = null;
            }
        }

        //Brake
        if (brake >= .01f && fuelSystem.HaveFuel())
        {
            fuelSystem.ConsumeFuel(Mathf.Abs(brake));
            MoveDeceleration(horizontalAxe, angularDeceleration, verticalAxe, brakeForce * brake);
        }

        if (InputController.Instance.Cancel)
        {
            EscEvent();
        }

        //Engines Animations
        if (fuelSystem.HaveFuel())
        {
            //if (Mathf.Abs(verticalAxe) >= .01f)
                EngineAnimations(verticalAxe);
            if (brake >= .01f)
                EngineAnimations(-brake);
        }
        else
            EngineAnimations(0);

        //Move deceleration
        if (!isInGravity/* && _rigidBody.velocity.magnitude > 0*/)
        {
            MoveDeceleration(horizontalAxe, angularDeceleration, verticalAxe, moveDeceleration);
            //if (verticalAxe == 0/* && horizontalAxe == 0*/)
            //{
            //    if (_rigidBody.velocity.magnitude < 0.1f)
            //    {
            //        _rigidBody.velocity = Vector2.zero;
            //        AngularDeveleration(horizontalAxe, angularDeceleration);
            //        return;
            //    }
            //    force = -_rigidBody.velocity.normalized * moveDeceleration;
            //    force = new Vector3(force.x, force.y, 0);
            //    Move(force, _transform.position);
            //}
        }

        //Rotate deceleration
        AngularDeceleration(horizontalAxe, angularDeceleration);
    }

    private void MoveDeceleration(float horizontalAxe, float angularDeceleration, float verticalAxe, float moveDeceleration)
    {
        if (_rigidBody.velocity.magnitude > 0)
        {
            if (verticalAxe == 0 || !fuelSystem.HaveFuel())
            {
                if (_rigidBody.velocity.magnitude < 0.1f)
                {
                    _rigidBody.velocity = Vector2.zero;
                    AngularDeceleration(horizontalAxe, angularDeceleration);
                    return;
                }
                Vector3 force = Vector3.zero;
                force = -_rigidBody.velocity.normalized * moveDeceleration;
                force = new Vector3(force.x, force.y, 0);
                Move(force, _transform.position);
            }
        }

        //Rotate deceleration
        AngularDeceleration(horizontalAxe, angularDeceleration);
    }

    private void AngularDeceleration(float horizontalAxe, float angularDeceleration)
    {
        //if (horizontalAxe == 0)
        //    print("angular " + _rigidBody.angularVelocity);
        if (horizontalAxe == 0 && _rigidBody.angularVelocity > 0)
        {
            if (_rigidBody.angularVelocity < angleLimitForAngularDeceleration)
            {
                _rigidBody.angularVelocity = 0;
                return;
            }
            _rigidBody.AddTorque(-angularDeceleration, ForceMode2D.Force);
        }
        else if (horizontalAxe == 0 && _rigidBody.angularVelocity < 0)
        {
            if (_rigidBody.angularVelocity > -angleLimitForAngularDeceleration)
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

    private void EngineAnimations(float verticalAxe)
    {
        engine1.SetFloat("engine", verticalAxe);
        engine2.SetFloat("engine", verticalAxe);
    }
}
