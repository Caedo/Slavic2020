using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public CharacterEntity player;

    public float attacksInterval;
    public BossAttackSequence[] attacks;

    void Awake() {
    }

    void Start() {
        StartCoroutine(AttacksRoutine());
    }

    IEnumerator AttacksRoutine() {
        while(true) {
            var index = Random.Range(0, attacks.Length);
            var attack = attacks[index];

            attack.StartSequence(player, this);

            yield return new WaitUntil(() => attack.isFinished);
            yield return new WaitForSeconds(attacksInterval);
        }
    }
}
