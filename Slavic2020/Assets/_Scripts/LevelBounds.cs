using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBounds : MonoBehaviour
{
    public Vector2 size;
    public Vector2 origin;

    public Vector2 FindEmptyPosition() {
        Vector2 result;

        result.x = origin.x + Random.Range(-size.x / 2, size.x / 2);
        result.y = origin.y + Random.Range(-size.x / 2, size.x / 2);

        return result;
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireCube(origin, size * 2);
    }
}
