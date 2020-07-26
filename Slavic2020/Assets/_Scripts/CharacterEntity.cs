using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEntity : MonoBehaviour {

    public PointsText pointsTextPrefab;

    [Space]
    public Transform bossTransform;
    public Transform arrowTransform;

    [Space]
    public float energyGainTime;
    public FloatVariable playerMaxEnergy;
    public FloatVariable playerEnergy;

    [Space]
    public PlayerBullet bulletPrefab;
    public float bulletsPerSecond;
    public float bulletEnergyCost;

    [Space]
    public GameEvent playerKilledEvent;
    public GameEvent playerEnerygyChangedEvent;

    float timeBetweenBullets;
    float bulletsTimer;

    new Camera camera;

    bool canGetEnergy;
    float energyTimer;

    void Awake() {
        camera = Camera.main;
        timeBetweenBullets = 1 / bulletsPerSecond;

        playerEnergy.Value = playerMaxEnergy.Value;
    }

    void Update() {
        bulletsTimer += Time.deltaTime;
        if(bulletsTimer >= timeBetweenBullets && Input.GetButton("Fire1")) {
            FireBullet();

            bulletsTimer = 0;
        }

        energyTimer += Time.deltaTime;
        if(energyTimer >= energyGainTime) {
            canGetEnergy = true;
        }

        if(bossTransform) {
            var dirToBoss = bossTransform.position - transform.position;
            var angle = Mathf.Atan2(dirToBoss.y, dirToBoss.x) * Mathf.Rad2Deg - 90f;
            
            arrowTransform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else {
            arrowTransform.rotation *= Quaternion.Euler(0, 0, 720f * Time.deltaTime);
        }
    }

    void FireBullet() {
        if(playerEnergy.Value <= 0) return;

        var screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -camera.transform.position.z);
        var worldPoint = camera.ScreenToWorldPoint(screenPoint);

        var delta = worldPoint - transform.position;
        var angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg - 90f;

        var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, angle));

        playerEnergy.Value -= bulletEnergyCost;
        playerEnerygyChangedEvent.Raise();
    }

    public void Kill() {
        playerKilledEvent.Raise();
        Destroy(gameObject);
    }

    public void GivePoints(float points) {
        if(canGetEnergy == false) return;

        playerEnergy.Value = Mathf.Min(playerEnergy.Value + points, playerMaxEnergy.Value);
        playerEnerygyChangedEvent.Raise();

        energyTimer = 0;
        canGetEnergy = false;

        var pointsText = Instantiate(pointsTextPrefab);
        pointsText.transform.position = transform.position;
        pointsText.Initialize((int)points);
    }
}
