using DG.Tweening;
using TMPro;
using UnityEngine;

public class DashInteraction : MonoBehaviour
{
    bool isFrozen = false;
    public TextMeshProUGUI dashText;
    AudioManager audioManager;

    private void Start()
    {
        dashText = GetComponentInChildren<TextMeshProUGUI>();
        audioManager = GameManager.instance.audioManager;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isFrozen = true;
            dashText.enabled = true;
            Time.timeScale = 0.0f;

            if (audioManager != null)
            {
                audioManager.musicSource.DOPitch(0.5f, 1.0f).SetUpdate(true);
                audioManager.effectSource.DOPitch(0.5f, 1.0f).SetUpdate(true);  
            }
        }
    }

  

    private void Update()
    {
        if (isFrozen && (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.JoystickButton5)))
        {
            UnfreezeTime();
        }
    }

    void UnfreezeTime()
    {
        Time.timeScale = 0.1f;
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, 2.0f).SetEase(Ease.OutCubic).SetUpdate(true);
        dashText.enabled = false;

        if (audioManager != null)
        {
            audioManager.musicSource.DOPitch(1.0f, 1.0f).SetUpdate(true);  // Restore full volume over 3s
            audioManager.effectSource.DOPitch(1.0f, 1.0f).SetUpdate(true);  // Restore full volume over 3s
        }

        Destroy(gameObject);
    }
}