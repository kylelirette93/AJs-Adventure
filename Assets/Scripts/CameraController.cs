using UnityEngine;


public class CameraController : MonoBehaviour
{
    [Header("Camera Controller Settings")]
    [SerializeField] Transform playerTransform;
    [SerializeField] float fixedY = 0f;
    float followSpeed = 20f;

    float smoothTime = 0.25f;
    Vector3 velocity = Vector2.zero;
    Vector3 playerPosition;
    

    void FixedUpdate()
    {
        // Get the player's position with a fixed Y position.
        playerPosition = new Vector3(playerTransform.position.x, fixedY, -10);

        // Smoothly move the camera towards the player.
        transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref velocity, smoothTime, followSpeed);
    }
}