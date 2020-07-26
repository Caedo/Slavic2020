using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsText : MonoBehaviour
{
    public float moveDist;
    public float moveTime;
    public float waitTime;

    TextMesh text;

    Vector3 startPos;
    Vector3 targetPos;

    public void Initialize(int pointsValue) {
        text = GetComponent<TextMesh>();
        text.text = pointsValue.ToString();

        Destroy(gameObject, 2f);

        startPos  = transform.position;

        var rot = Quaternion.Euler(0, 0, Random.Range(-40f, 40f));
        targetPos = startPos + rot * (Vector3.up * moveDist);

        StartCoroutine(MoveAnim());
    }

    float CubicEaseOut(float p) {
        float f = (p - 1);
        return f * f * f + 1;
    }

    IEnumerator MoveAnim() {
        for (float t = 0; t < moveTime; t += Time.deltaTime) {
            var p = CubicEaseOut(t / moveTime);

            transform.position = Vector3.Lerp(startPos, targetPos, p);

            yield return null;
        }

        Destroy(gameObject, waitTime);
    }
}
