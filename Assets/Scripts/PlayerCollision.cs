using UnityEngine;
public class PlayerCollision : MonoBehaviour
{
    new Rigidbody2D rigidbody2D;
    float bounceForce = 14f;

    private void Start()
    {
        rigidbody2D = GetComponentInParent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Headcheck")
        {
            BoxCollider2D collisionObjectCollider = other.gameObject.GetComponentInParent<BoxCollider2D>();
            if (collisionObjectCollider != null)
            {
                collisionObjectCollider.enabled = false;
            }
            GameManager.instance.audioManager.PlayOneShot(GameManager.instance.audioManager.bounceSFX);
            rigidbody2D.velocity = Vector2.up * bounceForce;
        }
    }
}
