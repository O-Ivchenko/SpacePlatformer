using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionTracker : Singleton<MissionTracker>
{
    public Transform Cargo;
    public Transform UnloadingPlatform;
    public Transform FuelStation;

    public Transform Target
    {
        get
        {
            if (isFuelWarning)
                return FuelStation;

            if (cargoTaken)
                return UnloadingPlatform;
            else
                return Cargo;
        }
        private set { }
    }

    private bool cargoTaken = false;
    private bool isFuelWarning = false;

    private void Start()
    {
        Target = Cargo;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Cargo = GameObject.Find("Cargo").transform;
        UnloadingPlatform = GameObject.Find("UnloadingPlatform").transform;
        FuelStation = GameObject.Find("FuelStation").transform;
        Target = Cargo;
        isFuelWarning = false;
        cargoTaken = false;
    }

    //public void SetFuelStation(Transform station)
    //{
    //    FuelStation = station;
    //}

    public void CargoTaken(bool isTaken)
    {
        cargoTaken = isTaken;
    }

    public void SetFuelWarning(bool warning)
    {
        isFuelWarning = warning;
    }
}
