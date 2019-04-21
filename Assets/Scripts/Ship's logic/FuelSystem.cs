using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelSystem : MonoBehaviour
{
    public Slider FuelIndicator;
    public int MaxCapacity = 100;
    public float FuelConsumption = .05f;
    public float StartCapacity = 5;
    public float fuelWarning = .3f;
    public Image canister;
    public float blinkTime= .5f;

    private float currentLevel;
    IEnumerator blinkCoroutine;

    private void Awake()
    {
        if (StartCapacity != 0)
            currentLevel = StartCapacity;
        else
            currentLevel = MaxCapacity;

        FuelIndicator.value = currentLevel / MaxCapacity;
        SetCanisterAlpha(0);
    }

    private void SetCanisterAlpha(float alpha)
    {
        Color c = canister.color;
        c.a = alpha;
        canister.color = c;
    }

    private IEnumerator CanisterBlink()
    {
        print(" CanisterBlink ");
        float blinkVelocity = 0;
        while (true)
        {
            while(canister.color.a > 0)
            {
                blinkVelocity = Time.deltaTime / blinkTime;
                SetCanisterAlpha(canister.color.a - blinkVelocity);
                yield return null;
            }
            while (canister.color.a < 1)
            {
                blinkVelocity = Time.deltaTime / blinkTime;
                SetCanisterAlpha(canister.color.a + blinkVelocity);
                yield return null;
            }
        }
    }

    public void ConsumeFuel(float gasPercentage)
    {
        if(HaveFuel())
        {
            currentLevel -= FuelConsumption * gasPercentage;
            FuelIndicator.value = currentLevel / MaxCapacity;
            if (FuelIndicator.value <= fuelWarning && blinkCoroutine == null)
            {
                blinkCoroutine = CanisterBlink();
                StartCoroutine(blinkCoroutine);
                MissionTracker.Instance.SetFuelWarning(true);
            }
            if (!HaveFuel())
                GameController.Instance.Lose();
        }
    }

    public bool HaveFuel()
    {
        if (currentLevel > 0)
            return true;
        else
            return false;
    }

    public void Fill(float amount)
    {
        if(currentLevel + amount < MaxCapacity)
        {
            currentLevel += amount;
            FuelIndicator.value = currentLevel / MaxCapacity;
            if (FuelIndicator.value >= fuelWarning && blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;
                MissionTracker.Instance.SetFuelWarning(false);
                SetCanisterAlpha(0);
            }
        }
        else if(currentLevel + amount >= MaxCapacity)
        {
            currentLevel = MaxCapacity;
            FuelIndicator.value = 1;
        }
    }

    public bool IsFullTank()
    {
        return currentLevel >= MaxCapacity;
    }
}
