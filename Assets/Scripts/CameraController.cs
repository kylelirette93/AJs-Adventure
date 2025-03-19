using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 0.1f;
    public Transform target;
    public float yOffset = 2f; 
    public float xOffset = 0f; 
    public float zOffset = -10f; 

    private void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                Debug.LogError("Player GameObject not found. Please ensure a GameObject named 'Player' exists in the scene.");
            }
        }
    }

    private void LateUpdate()
    {
        if (target == null) return; 

        Vector3 desiredPosition = target.position + new Vector3(xOffset, yOffset, zOffset);
        transform.position = Vector3.Slerp(transform.position, desiredPosition, cameraSpeed * Time.deltaTime);
    }
}