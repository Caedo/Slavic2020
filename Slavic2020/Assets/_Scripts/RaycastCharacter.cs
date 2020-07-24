using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct CollisionInfo {
    public bool top;
    public bool bot;
    public bool left;
    public bool right;

    public void Reset() {
        top   = false;
        bot   = false;
        left  = false;
        right = false;
    }

    public bool IsGrouded() {
        return bot;
    }

    public bool IsByWall() {
        return left || right;
    }
}

public class RaycastCharacter : MonoBehaviour
{
    const float skinWidth = 0.015f;

    public int rayCount = 5;
    public LayerMask platformMask;

    new Collider2D collider;
    Bounds bounds;

    Vector2 velocity;

    Vector2 topLeft;
    Vector2 topRight;
    Vector2 botLeft;
    Vector2 botRight;

    float horizontalStep;
    float verticalStep;

    CollisionInfo collisionInfo;

    void Awake() {
        collider = GetComponent<Collider2D>();
        bounds = collider.bounds;
    }

    void UpdateBounds() {
        var pos = transform.position;
        var verticalExtents   = bounds.extents.y - skinWidth;
        var horizontalExtents = bounds.extents.x - skinWidth;

        topLeft.x = pos.x - horizontalExtents;
        topLeft.y = pos.y + verticalExtents;

        topRight.x = pos.x + horizontalExtents;
        topRight.y = pos.y + verticalExtents;

        botLeft.x = pos.x - horizontalExtents;
        botLeft.y = pos.y - verticalExtents;

        botRight.x = pos.x + horizontalExtents;
        botRight.y = pos.y - verticalExtents;

        horizontalStep = (bounds.size.x - skinWidth * 2) / (rayCount - 1);
        verticalStep   = (bounds.size.y - skinWidth * 2) / (rayCount - 1);
    }

    void Update() {
        UpdateBounds();

        if(Input.GetKeyDown(KeyCode.Space)) {

            if(collisionInfo.IsGrouded()) {
                velocity.y = 10f;
            }
            else if(collisionInfo.IsByWall()) {
                velocity.x += collisionInfo.left ? 5 : -5;
            }
        }

        velocity += Physics2D.gravity * Time.deltaTime;

        var horizontal = Input.GetAxisRaw("Horizontal");
        velocity.x = horizontal * 5;

        collisionInfo.Reset();

        MoveHorizontal();
        MoveVertical();

        transform.Translate(velocity * Time.deltaTime);
    }

    void MoveHorizontal() {
        var t = Time.deltaTime;

        var dir = Mathf.Sign(velocity.x);

        float rayLength = Mathf.Abs(velocity.x) * t + skinWidth;
        var rayOrigin = dir == 1 ? botRight : botLeft;
        var rayDirection = new Vector2(dir, 0);

        for(int i = 0; i < rayCount; i++) {

            Debug.DrawRay(rayOrigin, rayDirection * rayLength * 2, Color.red);

            var hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, platformMask);
            if(hit) {
                rayLength  = hit.distance;
                velocity.x = (hit.distance - skinWidth) * dir;

                if(dir == 1) {
                    collisionInfo.right = true;
                }
                else {
                    collisionInfo.left = true;
                }
            }

            rayOrigin.y += verticalStep;
        }
    }

    void MoveVertical() {
        var t = Time.deltaTime;

        var dir = Mathf.Sign(velocity.y);

        float rayLength = Mathf.Abs(velocity.y) * t + skinWidth;
        var rayOrigin = dir == 1 ? topLeft : botLeft;
        var rayDirection = new Vector2(0, dir);

        for(int i = 0; i < rayCount; i++) {

            Debug.DrawRay(rayOrigin, rayDirection * rayLength * 2, Color.red);

            var hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, platformMask);
            if(hit) {
                rayLength  = hit.distance;
                velocity.y = (hit.distance - skinWidth) * dir;

                if(dir == 1) {
                    collisionInfo.top = true;
                }
                else {
                    collisionInfo.bot = true;
                }
            }

            rayOrigin.x += horizontalStep;
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, velocity * 0.1f);
    }
}
