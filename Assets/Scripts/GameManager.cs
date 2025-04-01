using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameStateManager gameStateManager;
    public UIManager uiManager;
    public AudioManager audioManager;
    public ScoreManager scoreManager;   

    private void Awake()
    {
        // Singleton pattern.
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
