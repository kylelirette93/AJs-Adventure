using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject GameplayPanel;
    public GameObject GameOverPanel;

    public void DisableAllMenuUI()
    {
        MenuPanel.SetActive(false);
        GameplayPanel.SetActive(false);
        GameOverPanel.SetActive(false);
    }

    public void EnableMainMenuUI()
    {
        MenuPanel.SetActive(true);
    }

    public void EnableGameplayUI()
    {
        GameplayPanel.SetActive(true);
    }

    public void EnableGameOverUI()
    {
        GameOverPanel.SetActive(true);
    }
}
