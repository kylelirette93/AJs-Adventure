using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI remainingText;
    float gameTime;

    private void Update()
    {
        if (GameManager.instance.gameStateManager.CurrentState == GameStateManager.GameState.Gameplay)
        {
            remainingText.text = "";
            gameTime = GameManager.instance.gameStateManager.GetRemainingTime();
            timerText.text = "Time left: " + gameTime.ToString("F2");
        }
        else if (GameManager.instance.gameStateManager.CurrentState == GameStateManager.GameState.GameWin)
        {
            remainingText.text = "You made it home in " + gameTime.ToString("F2") + " seconds.";
        }
        else if (GameManager.instance.gameStateManager.CurrentState == GameStateManager.GameState.GameOver)
        {
            remainingText.text = "You ran out of time! You were " + gameTime.ToString("F2") + " seconds away from home.";
        }
        else if (GameManager.instance.gameStateManager.CurrentState == GameStateManager.GameState.Menu || GameManager.instance.gameStateManager.CurrentState == GameStateManager.GameState.Difficulty)
        {
            timerText.text = "";
            remainingText.text = "";
        }
    }
}
