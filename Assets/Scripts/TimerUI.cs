using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    float timeCountdown;
    public TextMeshProUGUI remainingText;
    float gameTime;
    private Coroutine timerCoroutine;

    private void Update()
    {
        if (GameManager.instance.gameStateManager.CurrentState == GameStateManager.GameState.Gameplay)
        {
            remainingText.text = "";
            remainingText.text = "";
            gameTime = GameManager.instance.gameStateManager.GetRemainingTime();

            if (timerCoroutine == null)
            {
                timerCoroutine = StartCoroutine(UpdateTimerText());
            }
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
        else
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }

    IEnumerator UpdateTimerText()
    {
        float displayTime = gameTime;
        float lastDisplayedTime = -1f; 

        while (displayTime >= 0)
        {
            displayTime = GameManager.instance.gameStateManager.GetRemainingTime();

            if (Mathf.Abs(displayTime - lastDisplayedTime) >= 0.05f)
            {
                timerText.text = " Time: " + displayTime.ToString("F2");
                lastDisplayedTime = displayTime;
            }

            yield return new WaitForSeconds(0.02f); 
        }
    }
}
