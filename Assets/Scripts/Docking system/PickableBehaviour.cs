using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableBehaviour : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (/*Input.GetKeyDown(KeyCode.E) && */collision.tag == "Pickable")
        {
            print("pickable");
            FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
            joint.connectedBody = collision.GetComponent<Rigidbody2D>();
        }
    }

}
