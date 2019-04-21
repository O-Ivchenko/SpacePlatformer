using System;
using System.Collections.Generic;
using UnityEngine;

public class UnloadingBehaviour : MonoBehaviour
{
    public static Action MissionComplete;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Pickable")
        {
            MissionComplete();
        }
    }
}
