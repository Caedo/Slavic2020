using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public Vector2 focusBoxSize;

    Vector2 focusBoxPosition;

    void LateUpdate() {
        if(target == null) return;

        var left  = focusBoxPosition.x - focusBoxSize.x / 2f;
        var right = focusBoxPosition.x + focusBoxSize.x / 2f;
        var top   = focusBoxPosition.y + focusBoxSize.y / 2f;
        var bot   = focusBoxPosition.y - focusBoxSize.y / 2f;

        var pos = target.position;

        if(pos.x > right) {
            focusBoxPosition.x = pos.x - focusBoxSize.x / 2f;
        }
        else if (pos.x < left) {
            focusBoxPosition.x = pos.x + focusBoxSize.x / 2f;
        }

        if(pos.y > top) {
            focusBoxPosition.y = pos.y - focusBoxSize.y / 2f;
        }
        else if (pos.y < bot) {
            focusBoxPosition.y = pos.y + focusBoxSize.y / 2f;
        }

        transform.position = new Vector3(focusBoxPosition.x, focusBoxPosition.y, -10);
    }

    void OnDrawGizmos() {
        var color = Color.red;
        color.a = 0.4f;
        Gizmos.color = color;

        Vector3 size = focusBoxSize;
        size.z = 1;
        Gizmos.DrawCube(focusBoxPosition, size);
    }

}
