using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceBehaviour : MonoBehaviour
{
    public Vector3 force = Vector3.zero;
    //public Vector3 torque = Vector3.zero;

    private Rigidbody rig;

    private void Start()
    {
        rig = GetComponent<Rigidbody>();
        if (rig == null)
            rig = gameObject.AddComponent<Rigidbody>();

        AddForce();
    }

    private void FixedUpdate()
    {
        //AddForce();
    }

    private void AddForce()
    {
        rig.AddRelativeForce(force, ForceMode.Force);
        //rig.AddTorque(torque, ForceMode.Force);
    }
}
