using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : Singleton<UIController>
{
    public GameObject PauseMenu;
    public GameObject WinMenu;
    public GameObject LoseMenu;

    private void Start()
    {
        CloseAllMenus();
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
        CloseAllMenus();
    }

    private void CloseAllMenus()
    {
        PauseMenu.SetActive(false);
        WinMenu.SetActive(false);
        LoseMenu.SetActive(false);
    }

    public void OpenPauseMenu()
    {
        CloseAllMenus();
        PauseMenu.SetActive(true);
    }

    public void OpenWinMenu()
    {
        CloseAllMenus();
        WinMenu.SetActive(true);
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToTheGame()
    {
        PauseMenu.SetActive(false);
        GameController.Instance.UnPause();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void OpenLoseMenu()
    {
        CloseAllMenus();
        LoseMenu.SetActive(true);
    }
}
