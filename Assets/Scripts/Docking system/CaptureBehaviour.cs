using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureBehaviour : MonoBehaviour
{
    public static event Action DockedEvent;
    private FixedJoint2D joint;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Pickable" && joint == null && GravityRayWithCatchHands2.canTakeCargo == true)
        {
            print("pickable");
            joint = gameObject.AddComponent<FixedJoint2D>();
            joint.connectedBody = collision.GetComponent<Rigidbody2D>();
            DockedEvent();
        }
    }
}
