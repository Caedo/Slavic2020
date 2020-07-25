using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAttackSequence : ScriptableObject {

    public bool isFinished;

    public void StartSequence(CharacterEntity player, BossController boss) {
        isFinished = false;
        boss.StartCoroutine(AttackRoutine(player, boss));
    }

    public abstract IEnumerator AttackRoutine(CharacterEntity player, BossController boss);
}
