using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RunInteraction : MonoBehaviour
{
    public TextMeshProUGUI runText;


    private void Start()
    {
        runText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            runText.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            runText.enabled = false;
            Destroy(gameObject);
        }
    }
}
