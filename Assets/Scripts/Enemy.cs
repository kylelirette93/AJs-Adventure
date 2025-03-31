using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class Enemy : MonoBehaviour
{
    [Header("Base Enemy Settings")]
    [SerializeField] protected float movementSpeed;
    public Vector2 StartPosition { get { return startPosition; } }
    [SerializeField] protected Vector2 startPosition;
    [SerializeField] protected float travelDistance;

    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rigidbody2D;
    protected Vector2 moveDirection;
    protected bool movingLeft = true;
    public HealthSystem healthSystem = new HealthSystem(100);
    bool canMove = true;
    private Object enemyRef;
    bool isDead = false;
    BoxCollider2D collider;

    protected void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        startPosition = transform.position;
    }

    public virtual void Update()
    {
        if (canMove)
        {
            MoveEnemy();
        }
       
        if (transform.position.y < -10f && !isDead)
        {
            KillSelf();
        }
    }

    public virtual void MoveEnemy()
    {
        if (movingLeft && transform.position.x <= startPosition.x - travelDistance)
        {
            movingLeft = false;
            Flip();
        }
        else if (!movingLeft && transform.position.x >= startPosition.x + travelDistance)
        {
            movingLeft = true;
            Flip();
        }
        float direction = movingLeft ? -1 : 1;
        moveDirection = new Vector2(direction, 0);
        rigidbody2D.velocity = moveDirection * movementSpeed;        
    }

    void KillSelf()
    {
        isDead = true;
        gameObject.SetActive(false);

        Invoke("Respawn", 5f);
    }

    private void Respawn()
    {
        collider.enabled = true;
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY;
        canMove = true;
        isDead = false;
        gameObject.transform.position = startPosition;
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.SetActive(true);
    }

    public void Die()
    {
        canMove = false;
    }

    protected void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
