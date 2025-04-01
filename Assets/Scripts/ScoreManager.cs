using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    int cheeseCollected = 0;
    public TextMeshProUGUI cheeseCollectedText;

    public void AddCheese()
    {
        cheeseCollected++;
        cheeseCollectedText.text = "cheese: " + cheeseCollected.ToString();
    }

    public void Reset()
    {
        cheeseCollected = 0;
        cheeseCollectedText.text = "cheese: " + cheeseCollected.ToString();
    }
}
