using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    public Vector3 PointA;
    public Vector3 PointB;
    public float platformSpeed = 5f;

    private void Update()
    {
        // Smooth oscillation between 0 and 1.
        float t = Mathf.PingPong(Time.time * platformSpeed, 1);

        // Smoothly move the object between PointA and PointB.
        transform.position = Vector3.Lerp(PointA, PointB, t);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(gameObject.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}