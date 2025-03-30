using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheese : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private ParticleSystem particleSystem;
    bool isCollected = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        particleSystem = GetComponent<ParticleSystem>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isCollected)
            {
                GameManager.instance.scoreManager.AddCheese();
                GameManager.instance.audioManager.PlayOneShot(GameManager.instance.audioManager.eatSFX);
                isCollected = true;
            }         
            Explode();
        }
    }

    private void Explode()
    {
        particleSystem.Play();
        spriteRenderer.enabled = false; // Hide the sprite

        StartCoroutine(WaitForParticleSystemToFinish());
    }

    private IEnumerator WaitForParticleSystemToFinish()
    {
        while (particleSystem.isPlaying)
        {
            yield return null;
        }

        DestroyCheese();
    }

    private void DestroyCheese()
    {
        Destroy(gameObject);
    }
}
