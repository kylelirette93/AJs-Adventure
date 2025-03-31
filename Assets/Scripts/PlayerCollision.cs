using DG.Tweening;
using UnityEngine;
public class PlayerCollision : MonoBehaviour
{
    new Rigidbody2D rigidbody2D;
    
    [SerializeField] float bounceForce = 14f;

    private void Start()
    {
        rigidbody2D = GetComponentInParent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Headcheck")
        {
            BoxCollider2D collisionObjectCollider = other.gameObject.GetComponentInParent<BoxCollider2D>();
            Enemy enemy = other.gameObject.GetComponentInParent<Enemy>();
            Rigidbody2D enemyRb = other.gameObject.GetComponentInParent<Rigidbody2D>();
            enemy.Die();
            if (collisionObjectCollider != null)
            {
                Animator enemyAnimator = other.gameObject.GetComponentInParent<Animator>();
                if (enemyAnimator != null)
                {
                    Destroy(enemyAnimator);
                }

                Transform parentTransform = other.gameObject.transform.parent;
                if (parentTransform != null)
                {
                    parentTransform.DORotate(new Vector3(0, 0, 180), 0.5f);
                }

                collisionObjectCollider.enabled = false;

                if (enemyRb != null)
                {
                    enemyRb.constraints = RigidbodyConstraints2D.None;
                }
            }
            GameManager.instance.audioManager.PlayOneShot(GameManager.instance.audioManager.bounceSFX);
            rigidbody2D.velocity = Vector2.up * bounceForce;
        }
    }
}
