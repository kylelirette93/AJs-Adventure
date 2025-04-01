using System.Collections;
using UnityEngine;

public class Cheese : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private ParticleSystem particleSystem;
    private BoxCollider2D boxCollider;
    public bool IsCollected { get { return isCollected; } set { isCollected = value; } }
    bool isCollected = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        particleSystem = GetComponent<ParticleSystem>();
        boxCollider = GetComponent<BoxCollider2D>();
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
        boxCollider.enabled = false;

        StartCoroutine(WaitForParticleSystemToFinish());
    }

    private IEnumerator WaitForParticleSystemToFinish()
    {
        while (particleSystem.isPlaying)
        {
            yield return null;
        }
    }

    public void EnableCheese()
    {
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
    }
}
