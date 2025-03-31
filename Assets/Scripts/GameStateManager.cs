using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public Vector2 playerPosition;

    public GameState CurrentState { get; private set; } = GameState.Menu;

    public enum GameState
    {
        Menu,
        Gameplay,
        GameWin
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
    }

    public void Update()
    {
        switch (CurrentState) 
        {
            case GameState.Menu:
               GameManager.instance.uiManager.EnableMainMenuUI();
               break;
            case GameState.Gameplay:
                GameManager.instance.uiManager.EnableGameplayUI();
                break;
            case GameState.GameWin:
                GameManager.instance.uiManager.EnableGameWinUI();
                break;            
        }
    }

    public void PlayGame()
    {
        ChangeState(GameState.Gameplay);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void WinGame()
    {
        ChangeState(GameState.GameWin);
    }
}
