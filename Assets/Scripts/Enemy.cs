using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class Enemy : MonoBehaviour
{
    [Header("Base Enemy Settings")]
    [SerializeField] protected float movementSpeed;
    [SerializeField] protected Vector2 startPosition;
    [SerializeField] protected float travelDistance;

    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rigidbody2D;
    protected Vector2 moveDirection;
    protected bool movingLeft = true;
    public HealthSystem healthSystem = new HealthSystem(100);

    protected void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    public virtual void Update()
    {
        MoveEnemy();
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

    protected void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
