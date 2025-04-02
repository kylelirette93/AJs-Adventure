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
            remainingText.text = "You made it home in with " + gameTime.ToString("F2") + " seconds remaining";
        }
        else if (GameManager.instance.gameStateManager.CurrentState == GameStateManager.GameState.GameOver)
        {
            remainingText.text = "You ran out of time!";
        }
        else if (GameManager.instance.gameStateManager.CurrentState == GameStateManager.GameState.Menu || GameManager.instance.gameStateManager.CurrentState == GameStateManager.GameState.Difficulty)
        {
            timerText.text = "";
            remainingText.text = "";
        }
    }
}
