using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject GameplayPanel;
    public GameObject DifficultyPanel;
    public GameObject GameWinPanel;
    public GameObject GameOverPanel;
    public GameObject ControlsPanel;


    public void DisableAllMenuUI()
    {
        MenuPanel.SetActive(false);
        GameplayPanel.SetActive(false);
        GameWinPanel.SetActive(false);
        DifficultyPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        ControlsPanel.SetActive(false);
    }

    public void EnableMainMenuUI()
    {
        DisableAllMenuUI();
        MenuPanel.SetActive(true);
    }

    public void EnableGameplayUI()
    {
        DisableAllMenuUI();
        GameplayPanel.SetActive(true);
    }

    public void EnableGameWinUI()
    {
        DisableAllMenuUI();
        GameWinPanel.SetActive(true);
    }

    public void EnableDifficultyUI()
    {
        DisableAllMenuUI();
        DifficultyPanel.SetActive(true);
    }

    public void EnableGameOverUI()
    {
       DisableAllMenuUI();
       GameOverPanel.SetActive(true);
    }

    public void EnableControlsUI()
    {
        DisableAllMenuUI();
        ControlsPanel.SetActive(true);
    }
}
