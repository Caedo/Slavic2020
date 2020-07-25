using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public LevelBounds levelBounds;
    public float moveSpeed = 1;
    public CharacterEntity player;

    public float attacksInterval;
    public BossAttackSequence[] attacks;

    [Space]
    public FloatVariable maxBossHp;
    public FloatVariable currentBossHp;
    public GameEvent bossHpChangedEvent;

    Vector2 targetPos;

    void Awake() {
        currentBossHp.Value = maxBossHp.Value;
    }

    void Start() {
        StartCoroutine(AttacksRoutine());

        targetPos = levelBounds.FindEmptyPosition();
    }

    void Update() {
        transform.position = Vector2.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
        if(Vector2.Distance(transform.position, targetPos) < 0.1f) {
            targetPos = levelBounds.FindEmptyPosition();
        }
    }

    IEnumerator AttacksRoutine() {
        while(player) {
            var index = Random.Range(0, attacks.Length);
            var attack = attacks[index];

            attack.StartSequence(player, this);

            yield return new WaitUntil(() => attack.isFinished);
            yield return new WaitForSeconds(attacksInterval);
        }
    }

    public void Damage(float damageValue) {
        currentBossHp.Value -= damageValue;
        bossHpChangedEvent.Raise();
    }
}
