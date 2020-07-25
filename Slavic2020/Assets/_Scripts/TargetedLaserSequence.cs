using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class TargetedLaserSequence: BossAttackSequence {
    public TargetedLaser laserPrefab;
    public float lasersCount;
    public float timeBetweenFires;

    public override IEnumerator AttackRoutine(CharacterEntity player, BossController boss) {
        for (int i = 0; i < lasersCount; i++) {
            var laser = Object.Instantiate(laserPrefab, boss.transform.position, Quaternion.identity);
            laser.Initialize(player);

            yield return new WaitForSeconds(timeBetweenFires);
        }

        isFinished = true;
    }
}