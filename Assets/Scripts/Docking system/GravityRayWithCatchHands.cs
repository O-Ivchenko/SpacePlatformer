using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityRayWithCatchHands : MonoBehaviour, IDockingMethod
{
    const int countOfCatchHands = 2;
    public static bool isCargoTaken = false;

    public float rotationLimit = 10;
    public float velocity = 3;
    public float G = 0.01f;
    public Transform gravityPoint;
    public FixedJoint2D leftHandJoint;
    public FixedJoint2D rightHandJoint;
    public SpriteRenderer pic;

    private GameObject cargo;
    private Rigidbody2D cargoRigidbody;
    private Rigidbody2D shipRigidbody;
    private Transform ship;
    private Transform _transform;
    private Transform leftHand;
    private Transform rightHand;
    private int dockedCount = 0;

    private void Awake()
    {
        _transform = transform;
        leftHand = leftHandJoint.transform.parent;
        rightHand = rightHandJoint.transform.parent;
        leftHand.gameObject.SetActive(false);
        rightHand.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        CaptureBehaviour.DockedEvent += Docked;
    }

    private void OnDisable()
    {
        CaptureBehaviour.DockedEvent -= Docked;
    }

    private void Docked()
    {
        dockedCount++;
        print("docked " + dockedCount);
    }

    void IDockingMethod.Setup(GameObject parent, Vector3 position)
    {
        ship = parent.transform;
        shipRigidbody = ship.GetComponent<Rigidbody2D>();
        _transform.localRotation = Quaternion.identity;
        _transform.localPosition = position;
        _transform.localScale = Vector3.one;
        leftHandJoint.connectedBody = shipRigidbody;
        rightHandJoint.connectedBody = shipRigidbody;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Pickable")
        {
            cargo = collision.gameObject;
            //cargo.transform.parent = ship;
            cargoRigidbody = cargo.GetComponent<Rigidbody2D>();
            StartCoroutine("Docking");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Pickable")
        {
            StopCoroutine("Docking");
            cargo = null;
            cargoRigidbody = null;
        }
    }

    private IEnumerator Docking()
    {
        //cargoRigidbody.velocity = offset.normalized * velocity;
        //cargoRigidbody.velocity = ship.GetComponent<Rigidbody2D>().velocity;
        float currentSize = leftHand.transform.localScale.x;
        while (dockedCount < countOfCatchHands/*currentSize <= 1.7f*/)
        {
            Vector3 offset = gravityPoint.position - cargo.transform.position;
            Vector3 gravity = offset.normalized * shipRigidbody.mass * cargoRigidbody.mass * G / offset.sqrMagnitude;

            if (cargoRigidbody.rotation <= shipRigidbody.rotation - rotationLimit)
            {
                //print("rotate left " + cargoRigidbody.rotation + " " + shipRigidbody.rotation);
                cargoRigidbody.MoveRotation(cargoRigidbody.rotation + 5);
            }
            else if (cargoRigidbody.rotation >= shipRigidbody.rotation + rotationLimit)
            {
                //print("rotate right " + cargoRigidbody.rotation + " " + shipRigidbody.rotation);
                cargoRigidbody.MoveRotation(cargoRigidbody.rotation - 5);
            }

            if (offset.magnitude >= .3f)
                cargoRigidbody.AddForce(gravity);
            else
            {
                if(!rightHand.gameObject.activeSelf)
                    rightHand.gameObject.SetActive(true);
                if (!leftHand.gameObject.activeSelf)
                    leftHand.gameObject.SetActive(true);
                //print("docked");
                currentSize += .2f;
                Vector3 scale = new Vector3(currentSize, leftHand.transform.localScale.y, leftHand.transform.localScale.z);
                leftHand.transform.localScale = scale;
                rightHand.transform.localScale = scale;
            }

            yield return null;
        }
        pic.enabled = false;
        isCargoTaken = true;
        //for(int i=0; i<=points.Length-1;i++)
        //{
        //    while(cargo.transform.localPosition.y <= points[i].localPosition.y)
        //    {
        //        cargoRigidbody.velocity = (points[i].position - cargo.transform.position).normalized * velocity;
        //        cargoRigidbody.angularVelocity = velocity;
        //        yield return new WaitForSeconds(.3f);
        //    }
        //}
        //cargoRigidbody.velocity = Vector2.zero;
    }

    void IDockingMethod.DestroyObject()
    {
        StopCoroutine("Docking");
        isCargoTaken = false;
        //cargo.transform.parent = null;
        Destroy(gameObject);
    }
}
