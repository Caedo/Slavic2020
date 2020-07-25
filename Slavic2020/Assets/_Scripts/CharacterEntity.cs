using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEntity : MonoBehaviour {

    public PlayerBullet bulletPrefab;
    public float bulletsPerSecond;

    public GameEvent playerKilledEvent;

    float timeBetweenBullets;
    float bulletsTimer;

    new Camera camera;

    void Awake() {
        camera = Camera.main;
        timeBetweenBullets = 1 / bulletsPerSecond;
    }

    void Update() {
        bulletsTimer += Time.deltaTime;
        if(bulletsTimer >= timeBetweenBullets && Input.GetButton("Fire1")) {
            FireBullet();

            bulletsTimer = 0;
        }
    }

    void FireBullet() {
        var screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -camera.transform.position.z);
        var worldPoint = camera.ScreenToWorldPoint(screenPoint);

        var delta = worldPoint - transform.position;
        var angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg - 90f;

        var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, angle));
    }

    public void Kill() {
        playerKilledEvent.Raise();
        Destroy(gameObject);
    }

    public void GivePoints(float points) {
        Debug.Log("Points: " + points.ToString());
    }
}
