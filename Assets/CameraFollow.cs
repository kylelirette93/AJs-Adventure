using UnityEngine;


public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    float smoothTime = 0.25f;
    const float followSpeed = 20f;
    float fixedY = -1f;
    Vector3 velocity = Vector3.zero;
    Vector3 playerPosition;

    void Update()
    {
        // Get the player's position with a fixed Y position.
        playerPosition = new Vector3(playerTransform.position.x, fixedY, -10);

        // Smoothly move the camera towards the player.
        transform.position = playerPosition;
    }
}