using UnityEngine;

public class CameraController : MonoBehaviour
{
    float cameraSpeed = 0.1f;
    Transform target;
    [SerializeField] float yOffset;

    private void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    private void LateUpdate()
    {
        Vector3 newPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.Slerp(transform.position, newPosition + new Vector3(0, yOffset, 0), cameraSpeed);
    }
}
