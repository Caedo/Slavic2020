using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CollisionInfo {
    public bool top;
    public bool bot;
    public bool left;
    public bool right;

    public bool climbingSlope;

    public void Reset() {
        top   = false;
        bot   = false;
        left  = false;
        right = false;
        climbingSlope = false;
    }
}

public class RaycastCharacter : MonoBehaviour
{
    const float skinWidth = 0.015f;

    public int rayCount = 5;
    public LayerMask platformMask;

    [Space]
    public float moveSpeed;
    public float acceleration;
    [Range(0, 90)]
    public float maxClimbAngle;
    public float jumpHeight;
    public float jumpTime;

    [Space]
    public float wallSlideSpeed;
    public Vector2 wallClimbVelocity;
    public Vector2 wallLeapVelocity;

    [Space]
    public float dashDistance;
    public float dashTime;

    public CollisionInfo collisionInfo;

    new Collider2D collider;
    Bounds bounds;

    Vector2 velocity;

    Vector2 topLeft;
    Vector2 topRight;
    Vector2 botLeft;
    Vector2 botRight;

    float horizontalStep;
    float verticalStep;

    float gravity;
    float jumpVelocity;

    float dashVelocity;
    float timeLeftIndash;
    float dashDir;
    bool dashing;

    float velocityXSmoothing;

    void Awake() {
        collider = GetComponent<Collider2D>();
        bounds = collider.bounds;

        gravity = -(2 * jumpHeight) / (jumpTime * jumpTime);
        jumpVelocity = -gravity * jumpTime;

        dashVelocity = dashDistance / dashTime;
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
        var horizontal = Input.GetAxisRaw("Horizontal");
        var jump       = Input.GetButtonDown("Jump");
        var dash       = Input.GetButtonDown("Dash");

        velocity.y += gravity * Time.deltaTime;

        bool wallSliding = false;
        if((collisionInfo.left || collisionInfo.right) && !collisionInfo.bot) {
            velocity.y = -Mathf.Min(-velocity.y, wallSlideSpeed);
            wallSliding = true;
        }

        var wallDir = collisionInfo.left ? -1 : 1;
        if(jump) {
            if(wallSliding) {
                if(horizontal == wallDir) velocity = wallClimbVelocity;
                else                      velocity = wallLeapVelocity;

                velocity.x *= -wallDir;
            }
            else if(collisionInfo.bot) {
                velocity.y = jumpVelocity;
            }
        }

        float targetVelocityX = horizontal * moveSpeed;
        velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, acceleration);

        if(timeLeftIndash > 0) {
            timeLeftIndash -= Time.deltaTime;
            if(timeLeftIndash < 0) {
                dashing = false;
            }

            velocity.y = 0;
            velocity.x = dashVelocity * dashDir;
        }

        if(dash && !dashing) {
            dashing = true;
            dashDir = horizontal;

            timeLeftIndash = dashTime;
        }

        collisionInfo.Reset();

        var vel = velocity * Time.deltaTime;
        MoveHorizontal(ref vel);

        if(velocity.y != 0) {
            MoveVertical(ref vel);
        }

        if(collisionInfo.top || collisionInfo.bot) {
            velocity.y = 0;
        }

        transform.Translate(vel);
    }

    void ClimbSlopes(float slopeAngle, ref Vector2 vel) {
        var moveX = vel.x * Time.deltaTime;
        vel.y = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveX;
        vel.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveX * Mathf.Sign(vel.x);

        collisionInfo.bot = true;
        collisionInfo.climbingSlope = true;
    }

    void MoveHorizontal(ref Vector2 vel) {
        var t = Time.deltaTime;

        var dir = Mathf.Sign(vel.x);

        float rayLength = Mathf.Abs(vel.x) + skinWidth;
        var rayOrigin = dir == 1 ? botRight : botLeft;
        var rayDirection = new Vector2(dir, 0);

        for(int i = 0; i < rayCount; i++) {

            Debug.DrawRay(rayOrigin, rayDirection * rayLength * 10, Color.red);

            var hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, platformMask);
            if(hit) {
                rayLength  = hit.distance;
                vel.x = (hit.distance - skinWidth) * dir;

                if(dir == 1) {
                    collisionInfo.right = true;
                }
                else {
                    collisionInfo.left = true;
                }

                // float angle = Vector2.Angle(Vector2.up, hit.normal);
                // if(i == 0 && angle < maxClimbAngle) {
                //     var distToSlope = hit.distance - skinWidth;
                //     //vel.x -= distToSlope * dir;

                //     ClimbSlopes(angle, ref vel);
                //     // vel.x += distToSlope * dir;
                //     break;
                // }
            }

            rayOrigin.y += verticalStep;
        }
    }

    void MoveVertical(ref Vector2 vel) {
        var t = Time.deltaTime;

        var dir = Mathf.Sign(vel.y);

        float rayLength = Mathf.Abs(vel.y) * t + skinWidth;
        var rayOrigin = dir == 1 ? topLeft : botLeft;
        var rayDirection = new Vector2(0, dir);

        for(int i = 0; i < rayCount; i++) {

            Debug.DrawRay(rayOrigin, rayDirection * rayLength * 10, Color.red);

            var hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, platformMask);
            if(hit) {
                rayLength  = hit.distance;
                vel.y = (hit.distance - skinWidth) * dir;

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
