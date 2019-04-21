using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public FixedJoint2D ropeAnchor;
    public Rigidbody2D connector;
    public float attractForce = 1;
    public float radiusForDelChain = 0.1f;

    private GameObject _go;
    private Transform _transform;
    private Rigidbody2D[] chains;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _go = gameObject;
        _transform = transform;
        _transform.localPosition = new Vector3(0, 0, _transform.localPosition.z);
        _transform.localRotation = Quaternion.identity;
        _transform.localScale = Vector3.one;
        chains = _transform.Find("Chains").GetComponentsInChildren<Rigidbody2D>();
    }

    public void Setup(Rigidbody2D anchor, Vector3 force)
    {
        ropeAnchor.connectedBody = anchor;
        connector.AddForce(force, ForceMode2D.Impulse);
    }

    public void DestroyRope(Vector2 force)
    {
        StartCoroutine(DestroyCoroutine(force));
    }

    private IEnumerator DestroyCoroutine(Vector2 force)
    {
        int i = 1;
        Vector3 pos = chains[0].transform.localPosition;
        yield return null;
        //ropeAnchor.enabled = false;
        //while(connector.transform.localPosition.y <= pos.y)
        //{
        //    print("move");
        //    chains[0].AddForce(force*10, ForceMode2D.Force);
        //    yield return new WaitForSeconds(.3f);
        //}
        //while (i <= chains.Length-1)
        //{
        //    //Destroy(chains[i - 1].gameObject);
        //    //chains[i].transform.localPosition = pos;
        //    //yield return new WaitForSeconds(.3f);
        //    Vector2 force = (pos - chains[i].transform.position).normalized * attractForce;
        //    Destroy(chains[i - 1].gameObject);
        //    chains[i].AddForce(force, ForceMode2D.Impulse);
        //    while (Vector2.Distance(chains[i].transform.position, pos) > radiusForDelChain)
        //    {
        //        print("Dist" + Vector2.Distance(chains[i].transform.position, pos));
        //        yield return null;
        //    }
        //    i++;

        //}
        Destroy(_go);
    }
}
