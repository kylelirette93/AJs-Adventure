using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public Vector2 playerPosition;

    float gameTimer;
    public bool IsGameOver = false;
    private bool isTimerRunning = false;
    public PlayerController player;

    public GameState CurrentState { get; private set; } = GameState.Menu;

    public enum GameState
    {
        Menu,
        Difficulty,
        Gameplay,
        GameWin,
        GameOver
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        if (newState == GameState.Gameplay)
        {
            isTimerRunning = true;
            GameManager.instance.audioManager.PlayMusic(GameManager.instance.audioManager.level1Music);
        }
        else
        {
            isTimerRunning = false;
        }
    }

    public void Update()
    {
        switch (CurrentState) 
        {
            case GameState.Menu:
               GameManager.instance.uiManager.EnableMainMenuUI();
               break;
            case GameState.Difficulty:
                GameManager.instance.uiManager.EnableDifficultyUI();
                break;
            case GameState.Gameplay:
                GameManager.instance.uiManager.EnableGameplayUI();
                StartGameTimer();
                break;
            case GameState.GameWin:
                player.ResetPlayer();
                GameManager.instance.uiManager.EnableGameWinUI();
                break;
            case GameState.GameOver:
                player.ResetPlayer();
                GameManager.instance.uiManager.EnableGameOverUI();
                break;
        }
    }

    private void StartGameTimer()
    {
        if (isTimerRunning && gameTimer > 0)
        {
            gameTimer -= Time.deltaTime;
            if (gameTimer < 0)
            {
                gameTimer = 0;
                ChangeState(GameState.GameOver);
            }
        }
    }
    public void PlayGame()
    {
        ChangeState(GameState.Difficulty);
    }

    public void EasySelect()
    {
        gameTimer = 120f;
        ChangeState(GameState.Gameplay);
    }

    public void MediumSelect()
    {
        gameTimer = 60f;
        ChangeState(GameState.Gameplay);
    }

    public void HardSelect()
    {
        gameTimer = 30f;
        ChangeState(GameState.Gameplay);
    }

    public void ReturnToMainMenu()
    {
        ChangeState(GameState.Menu);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void WinGame()
    {
        ChangeState(GameState.GameWin);
    }

    public float GetRemainingTime()
    { 
        return gameTimer;
    }
}
