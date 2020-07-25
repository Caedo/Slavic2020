using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : BossAttack
{
    public Transform gfxTransform;

    public float rotationSpeed;

    [Space]
    public float killCircleRadius;
    public float closeCircleRadius;

    float moveSpeed;

    bool fired = false;

    void Update() {
        gfxTransform.Rotate(0, 0, Time.deltaTime * rotationSpeed);

        OverlapCircle(killCircleRadius, closeCircleRadius);

        if(fired) {
            transform.position += transform.up * moveSpeed * Time.deltaTime;
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, killCircleRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, closeCircleRadius);
    }

    public void Fire(float speed) {
        fired = true;
        moveSpeed = speed;
    }
}
