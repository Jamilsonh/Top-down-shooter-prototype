using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveUpgradeManager : MonoBehaviour
{
    public GameObject upgradePanel; // O painel com as op��es de upgrade
    public Image backgroundOverlay; // A imagem de sobreposi��o escura no fundo
    public Button upgradeOption1;
    public Button upgradeOption2;
    public Button upgradeOption3;
    public TextMeshPro descriptionText1; // Texto da descri��o do upgrade 1
    public TextMeshPro descriptionText2; // Texto da descri��o do upgrade 2
    public TextMeshPro descriptionText3; // Texto da descri��o do upgrade 3

    private WaveManager waveManager;
    private StandardUpgradeManager standardUpgradeManager;
    private SpecialUpgradeManager specialUpgradeManager;

    void Start() {
        waveManager = FindObjectOfType<WaveManager>();
        standardUpgradeManager = GetComponent<StandardUpgradeManager>();
        specialUpgradeManager = GetComponent<SpecialUpgradeManager>();

        upgradePanel.SetActive(false);
        backgroundOverlay.gameObject.SetActive(false); // Desativa a sobreposi��o no in�cio

        upgradeOption1.onClick.AddListener(() => ChooseUpgrade(1));
        upgradeOption2.onClick.AddListener(() => ChooseUpgrade(2));
        upgradeOption3.onClick.AddListener(() => ChooseUpgrade(3));
    }

    public void ShowUpgrades() {
        Time.timeScale = 0; // Pausa o jogo
        backgroundOverlay.gameObject.SetActive(true); // Ativa a sobreposi��o escura
        upgradePanel.SetActive(true); // Mostra o painel de upgrades

        int currentWave = waveManager.currentWave; // Obt�m a wave atual

        // Verifica se a wave atual � uma wave especial
        if (IsSpecialWave(currentWave)) {
            // Mostra upgrades especiais
            specialUpgradeManager.SetSpecialUpgrades(upgradeOption1, descriptionText1, upgradeOption2, descriptionText2, upgradeOption3, descriptionText3);
        }
        else {
            // Mostra upgrades padr�es
            standardUpgradeManager.SetStandardUpgrades(upgradeOption1, descriptionText1, upgradeOption2, descriptionText2, upgradeOption3, descriptionText3);
        }
    }

    void ChooseUpgrade(int option) {
        switch (option) {
            case 1:
                upgradeOption1.onClick.Invoke();
                break;
            case 2:
                upgradeOption2.onClick.Invoke();
                break;
            case 3:
                upgradeOption3.onClick.Invoke();
                break;
        }

        upgradePanel.SetActive(false);
        backgroundOverlay.gameObject.SetActive(false); // Desativa a sobreposi��o escura
        Time.timeScale = 1; // Retoma o jogo
        waveManager.StartNextWaveManual(); // Inicia manualmente a pr�xima wave
    }

    // Verifica se a wave atual � especial
    private bool IsSpecialWave(int wave) {
        return wave % 3 == 0; // Define que a cada 3 waves � uma wave especial (3, 6, 9, etc.)
    }
}
