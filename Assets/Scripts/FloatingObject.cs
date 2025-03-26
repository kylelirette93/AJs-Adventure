using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    float floatHeight = 0.3f;
    Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;    
    }

    private void Update()
    {
        Mathf.Sign(Mathf.Sin(Time.time));
        transform.position = new Vector3(originalPosition.x, originalPosition.y * floatHeight * Mathf.Sin(Time.time), 0);
    }
}
