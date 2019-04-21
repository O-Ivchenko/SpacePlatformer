using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelStation : MonoBehaviour
{
    public float fillVelocity = .05f;
    public float fuelVolume = 150f;

    private FuelSystem fuelSystem;
    private float currentFuelVolume = 0;

    private void Start()
    {
        currentFuelVolume = fuelVolume;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            fuelSystem = collision.GetComponent<FuelSystem>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player" && fuelSystem != null)
        {
            if (currentFuelVolume > 0)
            {
                fuelSystem.Fill(fillVelocity);
                currentFuelVolume -= fillVelocity;
            }
            else
            {
                //TODO: Add message that fuel tank is empty in this station
                print("empty station");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            fuelSystem = null;
        }
    }
}
