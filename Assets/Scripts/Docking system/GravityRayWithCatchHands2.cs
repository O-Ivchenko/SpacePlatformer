using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityRayWithCatchHands2 : MonoBehaviour, IDockingMethod
{
    const int countOfCatchHands = 2;
    //TODO: Remade somehow...
    public static bool canTakeCargo { get; private set; }

    public float rotationLimit = 10;
    public float G = 0.01f;
    public Transform gravityPoint;
    public Animator gravAnimator;
    public FixedJoint2D leftHandJoint;
    public FixedJoint2D rightHandJoint;

    private GameObject cargo;
    private Rigidbody2D cargoRigidbody;
    private Rigidbody2D shipRigidbody;
    private Transform ship;
    private Transform _transform;
    //private Transform leftHand;
    //private Transform rightHand;
    private int dockedCount = 0;

    private void Awake()
    {
        _transform = transform;
        //leftHand = leftHandJoint.transform.parent;
        //rightHand = rightHandJoint.transform.parent;
        //leftHand.gameObject.SetActive(false);
        //rightHand.gameObject.SetActive(false);
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
        gravAnimator.SetBool("TurnOn", true);
        leftHandJoint.connectedBody = shipRigidbody;
        rightHandJoint.connectedBody = shipRigidbody;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Pickable")
        {
            cargo = collision.gameObject;
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
        while (dockedCount < countOfCatchHands/*currentSize <= 1.7f*/)
        {
            Vector3 offset = gravityPoint.position - cargo.transform.position;
            Vector3 gravity = offset.normalized * shipRigidbody.mass * cargoRigidbody.mass * G / offset.sqrMagnitude;
            print("ups angle " + AngleBetweenUps());
            float angle = AngleBetweenUps();
            if (angle == 0 || Mathf.Abs(angle) >= rotationLimit)
                if (angle >= 0)
                    cargoRigidbody.MoveRotation(cargoRigidbody.rotation - 1);
                else
                    cargoRigidbody.MoveRotation(cargoRigidbody.rotation + 1);

            canTakeCargo = (offset.magnitude < .3f) && (angle != 0) && (Mathf.Abs(angle) <= rotationLimit);//(cargoRigidbody.rotation >= shipRigidbody.rotation - rotationLimit) && (cargoRigidbody.rotation <= shipRigidbody.rotation + rotationLimit);
            if (offset.magnitude >= .3f)
                cargoRigidbody.AddForce(gravity);
            else
            {
                //if(!rightHand.gameObject.activeSelf)
                //    rightHand.gameObject.SetActive(true);
                //if (!leftHand.gameObject.activeSelf)
                //    leftHand.gameObject.SetActive(true);
                ////print("docked");
                //currentSize += .2f;
                //Vector3 scale = new Vector3(currentSize, leftHand.transform.localScale.y, leftHand.transform.localScale.z);
                //leftHand.transform.localScale = scale;
                //rightHand.transform.localScale = scale;
            }

            yield return null;
        }
        MissionTracker.Instance.CargoTaken(true);
        canTakeCargo = false;
        gravAnimator.SetBool("TurnOn", false);
    }

    // return minimal(optimal for rotating cargo) angle between up vectors ship and cargo 
    private float AngleBetweenUps()
    {
        float angle = 0;
        angle = Vector3.Angle(cargo.transform.up, shipRigidbody.transform.up);
        if (AngleDir(cargo.transform.up, shipRigidbody.transform.up) < 0)
            angle = -angle;
        if (Mathf.Abs(angle) > 90)
            if (angle > 0)
                angle = -(180 - angle);
            else
                angle = 180 - Mathf.Abs(angle);
        return angle;
    }
    

    public static float AngleDir(Vector2 A, Vector2 B)
    {
        return -A.x * B.y + A.y * B.x;
    }

    //private void OnDrawGizmos()
    //{
    //    if (cargo != null)
    //    {
    //        //print("hit " + gravityPoint.position + " " + cargo.transform.position);
    //        Gizmos.DrawRay(shipRigidbody.transform.position, -shipRigidbody.transform.up * 7);
    //        Gizmos.DrawRay(cargo.transform.position, cargo.transform.up * 3);
    //    }
    //}

    void IDockingMethod.DestroyObject()
    {
        StartCoroutine("DestroyCoroutine");
    }

    private IEnumerator DestroyCoroutine()
    {
        StopCoroutine("Docking");
        MissionTracker.Instance.CargoTaken(false);
        canTakeCargo = false;
        gravAnimator.SetBool("TurnOn", false);
        yield return new WaitForSeconds(.7f);
        Destroy(gameObject);
    }
}
