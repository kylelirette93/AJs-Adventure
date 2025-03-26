
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Base Enemy Settings")]
    [SerializeField] Vector3 PointA;
    [SerializeField] Vector3 PointB;
    [SerializeField] float movementSpeed;

    Vector2 direction;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        // Smoothly oscillate between 0 and 1.
        float t = Mathf.PingPong(Time.time * movementSpeed, 1);

        // Smoothly move the object between PointA and PointB.
        Vector3 movement = Vector3.Lerp(PointA, PointB, t);
        transform.position = movement;

        // Get the facing direction.
        direction = transform.right;
        spriteRenderer.flipX = direction.x < 0;
    }
}
