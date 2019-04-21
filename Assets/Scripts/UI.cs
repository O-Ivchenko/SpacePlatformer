using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Text label;
    public Rigidbody2D ship;

    private void Update()
    {
        label.text = ship.velocity + " " + ship.angularVelocity.ToString("F2") + " " + ship.velocity.magnitude.ToString("F2");
    }
}
