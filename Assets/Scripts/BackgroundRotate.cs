using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRotate : MonoBehaviour
{
    public float rotationSpeed = 30f; // You can adjust the speed as needed.

    void Update()
    {
        // Rotate the image around the Z-axis
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
