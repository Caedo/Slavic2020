using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour {

    public GameOverScreen gameOverScreenPrefab;

    [Space]
    public FloatVariable bossMaxHp;
    public FloatVariable bossCurrentHp;
    public FloatVariable playerPoints;

    public FloatVariable playerMaxEnergy;
    public FloatVariable playerEnergy;

    [Space]
    public Image bossHpImage;
    public Image playerEnergyImage;
    public Text playerPointsText;

    public void ShowGameOverScreen() {
        Instantiate(gameOverScreenPrefab);
    }

    public void BossHpChanged() {
        bossHpImage.fillAmount = bossCurrentHp.Value / bossMaxHp.Value;
    }

    public void PlayerEnergyChanged() {
        playerEnergyImage.fillAmount = playerEnergy.Value / playerMaxEnergy.Value;
    }
}
