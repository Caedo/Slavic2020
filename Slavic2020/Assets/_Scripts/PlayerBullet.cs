using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {

    public float speed;
    public float damage;

    void Update() {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other) {
        var boss = other.GetComponent<BossController>();
        if(boss) {
            boss.Damage(damage);
        }

        Destroy(gameObject);
    }
}
