using UnityEngine;

public class BackgroundZoom : MonoBehaviour
{
    public float zoomSpeed = 0.5f;
    public float maxScale = 1.2f;
    public float minScale = 0.8f;

    private bool zoomingIn = true;

    void Update()
    {
        float scaleFactor = zoomingIn ? 1 + zoomSpeed * Time.deltaTime : 1 - zoomSpeed * Time.deltaTime;
        transform.localScale *= scaleFactor;

        if (transform.localScale.x > maxScale || transform.localScale.x < minScale)
        {
            zoomingIn = !zoomingIn;
        }
    }
}
