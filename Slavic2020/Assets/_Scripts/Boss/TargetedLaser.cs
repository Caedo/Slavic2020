using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetedLaser : BossAttack
{
    public float timeToStart;
    public float transitionTime;
    public float attackTime;

    [Space]
    public float startWidth;
    public float attackWidth;

    public float closeStartWidth;
    public float closeEndWidth;

    public LineRenderer line;
    public LineRenderer closeLine;

    Vector2 targetPoint;
    Vector3 targetDir;

    public void Initialize(CharacterEntity player) {
        if(player == null) return;

        var playerPos = player.transform.position;
        targetDir = (playerPos - transform.position).normalized;

        targetPoint = transform.position + targetDir * 250f;

        StartCoroutine(AttackRoutine());
    }

    void FireLaser(float width, float closeWidth) {
        CircleCast(targetDir, width, closeWidth);
    }

    IEnumerator AttackRoutine() {
        line.startWidth = startWidth;
        line.endWidth   = startWidth;

        closeLine.startWidth = closeStartWidth;
        closeLine.endWidth   = closeStartWidth;

        line.positionCount = 2;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, targetPoint);
        
        closeLine.positionCount = 2;
        closeLine.SetPosition(0, transform.position);
        closeLine.SetPosition(1, targetPoint);

        for (float  t = 0; t < timeToStart; t += Time.deltaTime) {
            yield return null;
        }

        for (float  t = 0; t < transitionTime; t += Time.deltaTime) {
            var width = Mathf.Lerp(startWidth, attackWidth, t / transitionTime);
            var closeWidth = Mathf.Lerp(closeStartWidth, closeEndWidth, t / transitionTime);

            line.startWidth = width;
            line.endWidth   = width;

            closeLine.startWidth = closeWidth;
            closeLine.endWidth   = closeWidth;
    
            yield return null;
        }


        line.startWidth = attackWidth;
        line.endWidth   = attackWidth;

        closeLine.startWidth = closeEndWidth;
        closeLine.endWidth   = closeEndWidth;

        for (float  t = 0; t < timeToStart; t += Time.deltaTime) {
            FireLaser(attackWidth, closeEndWidth);
            yield return null;
        }

        for (float  t = 0; t < transitionTime; t += Time.deltaTime) {
            var width = Mathf.Lerp(attackWidth, startWidth, t / transitionTime);
            var closeWidth = Mathf.Lerp(closeEndWidth, closeStartWidth, t / transitionTime);

            line.startWidth = width;
            line.endWidth   = width;

            closeLine.startWidth = closeWidth;
            closeLine.endWidth   = closeWidth;

            yield return null;
        }

        Destroy(gameObject);
    }
}
