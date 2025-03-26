using UnityEngine;


public class CameraController : MonoBehaviour
{
    [Header("Camera Controller Settings")]
    [SerializeField] Transform playerTransform;
    [SerializeField] float fixedY = 0f;

    Vector3 playerPosition;

    void Update()
    {
        // Get the player's position with a fixed Y position.
        playerPosition = new Vector3(playerTransform.position.x, fixedY, -10);

        // Smoothly move the camera towards the player.
        transform.position = playerPosition;
    }
}