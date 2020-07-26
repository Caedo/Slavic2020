using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAttack : MonoBehaviour
{
    public LayerMask playerMask;
    public LayerMask platformLayer;

    public int pointsValue;

    public void OverlapCircle(float killRadius, float closeRadius) {
        var collider = Physics2D.OverlapCircle(transform.position, killRadius, playerMask);
        if(collider) {
            var player = collider.GetComponent<CharacterEntity>();
            if(player) {
                player.Kill();
            }
        }

        collider = Physics2D.OverlapCircle(transform.position, closeRadius, playerMask);
        if(collider) {
            var player = collider.GetComponent<CharacterEntity>();
            if(player) {
                player.GivePoints(pointsValue);
            }
        }
    }

    public void CircleCast(Vector2 direction, float width, float closeWidth) {
        var hit = Physics2D.CircleCast(transform.position, width / 2f, direction, Mathf.Infinity, playerMask);
        if(hit) {
            var player = hit.collider.GetComponent<CharacterEntity>();
            if(player) {
                player.Kill();
                return;
            }
        }

        hit = Physics2D.CircleCast(transform.position, closeWidth / 2f, direction, Mathf.Infinity, playerMask);
        if(hit) {
            var player = hit.collider.GetComponent<CharacterEntity>();
            if(player) {
                player.GivePoints(pointsValue);
            }
        }
    }
}
