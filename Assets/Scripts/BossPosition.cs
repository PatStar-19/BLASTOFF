using System.Collections;
using UnityEngine;

public class BossPosition : MonoBehaviour
{
    // Flag to control whether the enemy should respawn
    public bool shouldRespawn = true;

    void Start()
    {
        // Invoke the OnDrawGizmos method every 5 seconds
        InvokeRepeating("DrawGizmosWithDelay", 0f, 5f);
    }

    void DrawGizmosWithDelay()
    {
        Gizmos.DrawWireSphere(transform.position, 1);
    }

    // Function to set the respawn flag
    public void SetRespawn(bool respawn)
    {
        shouldRespawn = respawn;
    }
}
