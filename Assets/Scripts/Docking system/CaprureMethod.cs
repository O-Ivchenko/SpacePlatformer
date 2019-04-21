using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaprureMethod : MonoBehaviour, IDockingMethod
{
    public float handSize = 1.5f;
    public FixedJoint2D leftHandJoint;
    public FixedJoint2D rightHandJoint;

    private Transform _transform;
    private Transform leftHand;
    private Transform rightHand;

    private void Awake()
    {
        _transform = transform;
        leftHand = leftHandJoint.transform.parent;
        rightHand = rightHandJoint.transform.parent;
    }

    void IDockingMethod.Setup(GameObject ship, Vector3 position)
    {
        _transform.localScale = Vector3.one;
        _transform.localRotation = Quaternion.identity;
        _transform.localPosition = position;
        Rigidbody2D shipRigidbody = ship.GetComponent<Rigidbody2D>();
        leftHandJoint.connectedBody = shipRigidbody;
        rightHandJoint.connectedBody = shipRigidbody;
        StartCoroutine("Capturing");
    }

    private IEnumerator Capturing()
    {
        float currentSize = leftHand.transform.localScale.x;
        while(currentSize <= handSize)
        {
            currentSize += .2f;
            Vector3 scale = new Vector3(currentSize, leftHand.transform.localScale.y, leftHand.transform.localScale.z);
            leftHand.transform.localScale = rightHand.transform.localScale = scale;
            yield return new WaitForSeconds(.1f);
        }
    }

    void IDockingMethod.DestroyObject()
    {
        StopCoroutine("Capturing");
        Destroy(gameObject);
    }
}
