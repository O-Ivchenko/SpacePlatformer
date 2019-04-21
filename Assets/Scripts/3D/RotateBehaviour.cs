using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBehaviour : MonoBehaviour
{
    public Vector3 torque = Vector3.zero;

    private Rigidbody rig;

    private void Start()
    {
        rig = GetComponent<Rigidbody>();
        if (rig == null)
            rig = gameObject.AddComponent<Rigidbody>();

        StartRotate();
    }

    private void FixedUpdate()
    {
        //StartRotate();
    }

    private void StartRotate()
    {
        rig.AddTorque(torque, ForceMode.Impulse);
    }
}
