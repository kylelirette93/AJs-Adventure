using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject GameplayPanel;
    public GameObject GameWinPanel;

    public void DisableAllMenuUI()
    {
        MenuPanel.SetActive(false);
        GameplayPanel.SetActive(false);
        GameWinPanel.SetActive(false);
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
}
