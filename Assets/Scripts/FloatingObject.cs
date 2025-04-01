using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    float floatHeight = 0.5f;
    Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;    
    }

    private void Update()
    {
        transform.position = new Vector3(
            originalPosition.x,
            originalPosition.y + (floatHeight * Mathf.Sin(Time.time)), // Corrected line
            originalPosition.z // Ensure to keep the original z position
        );
    }
}
