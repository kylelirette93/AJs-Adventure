using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    float length, startPos;
    GameObject cam;
    public float parallaxFactor;

    private void Start()
    {
        cam = Camera.main.gameObject;
        // Get the backgrounds start position.
        startPos = transform.position.x;
        // Get length of the sprite.
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxFactor));
        float distance = (cam.transform.position.x * parallaxFactor);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (temp > startPos + length) startPos += length;
        else if (temp < startPos - length) startPos -= length;
    }
}
