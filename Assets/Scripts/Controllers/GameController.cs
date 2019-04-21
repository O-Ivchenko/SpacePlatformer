using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : Singleton<GameController>
{
    //TODO: Separate game logic and UI logic
    public enum GameStates
    {
        Pause,
        Unpause,
        Win,
        Lose
    }

    public GameStates gameState = GameStates.Unpause;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SpaceShip4Controller2d.EscEvent += GamePause;
        UnloadingBehaviour.MissionComplete += Win;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SpaceShip4Controller2d.EscEvent -= GamePause;
        UnloadingBehaviour.MissionComplete -= Win;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1;
        gameState = GameStates.Unpause;
    }

    private void Win()
    {
        gameState = GameStates.Win;
        StartCoroutine("WinCoroutine");
    }

    private IEnumerator WinCoroutine()
    {
        yield return new WaitForSeconds(1);
        Time.timeScale = 0;
        UIController.Instance.OpenWinMenu();
    }

    private void GamePause()
    {
        if(gameState == GameStates.Pause)
        {
            UIController.Instance.ReturnToTheGame();
            return;
        }

        if (gameState != GameStates.Unpause)
            return;
        gameState = GameStates.Pause;
        Time.timeScale = 0;
        UIController.Instance.OpenPauseMenu();
    }

    public void UnPause()
    {
        if (gameState != GameStates.Pause)
            return;
        gameState = GameStates.Unpause;
        Time.timeScale = 1;
    }

    public void Lose()
    {
        gameState = GameStates.Lose;
        Time.timeScale = 0;
        UIController.Instance.OpenLoseMenu();
    }
}
