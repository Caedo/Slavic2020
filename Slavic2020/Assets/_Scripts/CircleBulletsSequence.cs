using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class CircleBulletsSequence: BossAttackSequence {
    public BossBullet bulletPrefab;

    public float bulletSpeed;
    public int bulletsCount = 12;

    [Space]
    public float prepareTime;
    public float prepareDistance;

    public override IEnumerator AttackRoutine(CharacterEntity player, BossController boss) {
        var bullets = new BossBullet[bulletsCount];
        for (int i = 0; i < bulletsCount; i++) {
            var angle = i * (360f / bulletsCount) - 90;

            var bullet = Instantiate(bulletPrefab, boss.transform.position, Quaternion.Euler(0, 0, angle));

            bullets[i] = bullet;
        }

        for(float t = 0; t < prepareTime; t += Time.deltaTime) {
            var p = (t / prepareTime);
            for(int i = 0; i < bulletsCount; i++) {
                var delta = bullets[i].transform.up * p * prepareDistance;
                bullets[i].transform.position = boss.transform.position + delta;
            }

            yield return null;
        }

        for(int i = 0; i < bulletsCount; i++) {
            bullets[i].Fire(bulletSpeed);
        }

        isFinished = true;
    }
}