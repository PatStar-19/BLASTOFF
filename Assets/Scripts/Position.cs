using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour
{
    // Flag to control whether the enemy should respawn
    public bool shouldRespawn = true;

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 1);
    }

    // Function to set the respawn flag
    public void SetRespawn(bool respawn)
    {
        shouldRespawn = respawn;
    }
}
