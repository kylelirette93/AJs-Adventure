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
    public InputManager inputManager;

    public GameState CurrentState { get; private set; } = GameState.Menu;

    public enum GameState
    {
        Menu,
        Controls,
        Difficulty,
        Gameplay,
        GameWin,
        GameOver
    }

    private void Start()
    {
        if (!GameManager.instance.audioManager.IsPlaying)
        {
            GameManager.instance.audioManager.PlayMusic(GameManager.instance.audioManager.mainMenuMusic);
        }
    }

    public void ChangeState(GameState newState)
    {
        if (CurrentState != newState)
        {
            ExitState(CurrentState);
            CurrentState = newState;
            EnterState(CurrentState);
        }
    }

    private void EnterState(GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                GameManager.instance.uiManager.EnableMainMenuUI();
                if (!GameManager.instance.audioManager.IsPlaying)
                GameManager.instance.audioManager.PlayMusic(GameManager.instance.audioManager.mainMenuMusic);
                inputManager.gameInput.Disable();
                break;

            case GameState.Controls:
                GameManager.instance.uiManager.EnableControlsUI();
                inputManager.gameInput.Disable();
                break;

            case GameState.Difficulty:
                GameManager.instance.uiManager.EnableDifficultyUI();
                inputManager.gameInput.Disable();
                break;

            case GameState.Gameplay:
                GameManager.instance.uiManager.EnableGameplayUI();
                GameManager.instance.audioManager.PlayMusic(GameManager.instance.audioManager.level1Music);
                inputManager.gameInput.Enable();
                isTimerRunning = true;
                break;

            case GameState.GameWin:
                player.ResetPlayer();
                GameManager.instance.uiManager.EnableGameWinUI();
                GameManager.instance.audioManager.StopMusic();
                inputManager.gameInput.Disable();
                break;

            case GameState.GameOver:
                player.ResetPlayer();
                GameManager.instance.uiManager.EnableGameOverUI();
                GameManager.instance.audioManager.StopMusic();
                inputManager.gameInput.Disable();
                break;
        }
    }

    private void ExitState(GameState state)
    {
        switch (state)
        {
            case GameState.Gameplay:
                isTimerRunning = false;
                break;
            case GameState.Menu:
                GameManager.instance.uiManager.DisableAllMenuUI();
                break;
            case GameState.Controls:
                GameManager.instance.uiManager.DisableAllMenuUI();
                break;
            case GameState.Difficulty:
                GameManager.instance.uiManager.DisableAllMenuUI();
                break;
            case GameState.GameWin:
                GameManager.instance.uiManager.DisableAllMenuUI();
                break;
            case GameState.GameOver:
                GameManager.instance.uiManager.DisableAllMenuUI();
                break;
        }
    }

    public void Update()
    {
        if (CurrentState == GameState.Gameplay)
        {
            StartGameTimer();
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

    public void Controls()
    {
        ChangeState(GameState.Controls);
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
