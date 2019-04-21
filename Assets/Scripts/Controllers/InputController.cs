using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : Singleton<InputController>
{
    public float HorizontalAxe { get; private set; }
    public float VerticalAxe { get; private set; }
    public bool KeyE { get; private set; }
    public bool KeyQ { get; private set; }
    public bool Cancel { get; private set; }
    public float Space { get; private set; }

    private void Update()
    {
        HorizontalAxe = Input.GetAxis("Horizontal");
        VerticalAxe = Input.GetAxis("Vertical");
        KeyE = Input.GetButtonDown("KeyE");
        KeyQ = Input.GetButtonDown("KeyQ");
        Cancel = Input.GetButtonDown("Cancel");
        Space = Input.GetAxis("Jump");
    }
}
