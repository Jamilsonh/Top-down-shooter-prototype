using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour {
    public GameObject upgradePanel;
    public Image backgroundOverlay;

    public Button upgradeOption1;
    public Button upgradeOption2;
    public Button upgradeOption3;

    // Nome, descrição e nível separados para cada upgrade
    public TextMeshProUGUI upgradeName1;
    public TextMeshProUGUI upgradeName2;
    public TextMeshProUGUI upgradeName3;

    public TextMeshProUGUI upgradeDescription1;
    public TextMeshProUGUI upgradeDescription2;
    public TextMeshProUGUI upgradeDescription3;

    public TextMeshProUGUI upgradeLevel1;
    public TextMeshProUGUI upgradeLevel2;
    public TextMeshProUGUI upgradeLevel3;

    public Image upgradeImage1;
    public Image upgradeImage2;
    public Image upgradeImage3;

    private WaveManager waveManager;
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;
    private BulletTypeManager bulletTypeManager;

    private bool dashUnlocked = false;
    private bool dashCooldownUpgradeAdded = false; // Variável para garantir que o cooldown do Dash seja adicionado uma vez

    private List<Upgrade> availableUpgrades = new List<Upgrade>();

    private List<Upgrade> commonUpgrades = new List<Upgrade>();
    private List<Upgrade> specialUpgrades = new List<Upgrade>();

    private Dictionary<string, int> upgradeCounts = new Dictionary<string, int>(); // Contagem de upgrades aplicados

    private Upgrade dashUpgrade;
    private Upgrade dashCooldownUpgrade;

    private bool specialUpgradesAdded = false;

    void Start() {
        Debug.Log("Iniciando UpgradeManager");
        foreach (var upgrade in upgradeCounts) {
            Debug.Log($"Upgrade: {upgrade.Key}, Nível: {upgrade.Value}");
        }
        waveManager = FindObjectOfType<WaveManager>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        bulletTypeManager = FindObjectOfType<BulletTypeManager>();

        upgradePanel.SetActive(false);
        backgroundOverlay.gameObject.SetActive(false);

        upgradeOption1.onClick.AddListener(() => ChooseUpgrade(1));
        upgradeOption2.onClick.AddListener(() => ChooseUpgrade(2));
        upgradeOption3.onClick.AddListener(() => ChooseUpgrade(3));

        PopulateDefaultUpgrades();
        PopulateSpecialUpgrades();
    }

    private void ChooseUpgrade(int v) {
        throw new System.NotImplementedException();
    }

    void PopulateDefaultUpgrades() {
        Sprite bulletDamageImage = Resources.Load<Sprite>("Images/bulletDamage");
        Sprite extraSpeedImage = Resources.Load<Sprite>("Images/extraSpeed");
        Sprite extraHealthImage = Resources.Load<Sprite>("Images/extraHealth");
        Sprite fireRateImage = Resources.Load<Sprite>("Images/fireRate");
        Sprite unlockDashImage = Resources.Load<Sprite>("Images/unlockDash");

        if (upgradeCounts.Count == 0) {  // Inicializa apenas se estiver vazio
            AddCommonUpgrade(new Upgrade("Aumento de Dano", "Aumenta o dano do tiro em 5 pontos", bulletDamageImage, () => bulletTypeManager.IncreaseBulletDamage(10f)), "Aumento de Dano");
            AddCommonUpgrade(new Upgrade("Velocidade Extra", "Aumenta a velocidade do player em 2 unidades", extraSpeedImage, () => playerMovement.IncreaseMoveSpeed(2f)), "Velocidade Extra");
            AddCommonUpgrade(new Upgrade("Vida Extra", "Aumenta a vida máxima do player em 20 pontos", extraHealthImage, () => playerHealth.IncreaseMaxHealth(20)), "Vida Extra");
            AddCommonUpgrade(new Upgrade("Taxa de Disparo", "Aumenta a taxa de disparo do player", fireRateImage, () => playerMovement.IncreaseFireRate(0.05f)), "Taxa de Disparo");

            dashUpgrade = new Upgrade("Dash", "Habilita a habilidade de dash", unlockDashImage, () => EnableDash());
            AddCommonUpgrade(dashUpgrade, "Dash");

            Debug.Log("Default upgrades inicializados.");
        }
        else {
            Debug.Log("UpgradeCounts já possui valores. Não reinicializando.");
        }


    }

    void AddCommonUpgrade(Upgrade upgrade, string upgradeName) {
        commonUpgrades.Add(upgrade);
        upgradeCounts[upgradeName] = 0; // Inicializa a contagem para o upgrade
    }

    void AddSpecialUpgrade(Upgrade upgrade, string upgradeName) {
        specialUpgrades.Add(upgrade);
        upgradeCounts[upgradeName] = 0; // Inicializa a contagem para o upgrade
    }

    public void ShowUpgrades() {
        // Atualiza upgrades disponíveis (caso o Dash tenha sido desbloqueado)
        if (dashUnlocked && !dashCooldownUpgradeAdded) {
            Sprite dashCooldownImage = Resources.Load<Sprite>("Images/dashCooldown");
            dashCooldownUpgrade = new Upgrade("Dash Rápido", "Reduz o tempo de recarga do dash", dashCooldownImage, () => ReduceDashCooldown(0.3f));
            AddCommonUpgrade(dashCooldownUpgrade, "Dash Rápido");
            dashCooldownUpgradeAdded = true;
        }

        Time.timeScale = 0;
        backgroundOverlay.gameObject.SetActive(true);
        upgradePanel.SetActive(true);

        List<Upgrade> upgradesToShow;

        // Escolhe a lista de upgrades com base na wave atual
        if (waveManager.currentWave == 5) {
            upgradesToShow = specialUpgrades;
        }
        else {
            upgradesToShow = commonUpgrades;
        }

        // Filtra e seleciona até 3 upgrades válidos da lista
        upgradesToShow = upgradesToShow
            .Where(u => upgradeCounts.ContainsKey(u.name) && upgradeCounts[u.name] < 3)
            .OrderBy(x => Random.value)
            .Take(3)
            .ToList();

        if (upgradesToShow.Count < 3) {
            // Fechar painel se não houver upgrades suficientes
            upgradePanel.SetActive(false);
            backgroundOverlay.gameObject.SetActive(false);
            Time.timeScale = 1;
            return;
        }

        // Exibe os upgrades na interface usando os níveis persistidos em upgradeCounts
        UpdateUpgradeDescriptions(upgradesToShow);

        // Configura os botões de upgrade para refletir a nova escolha
        upgradeOption1.onClick.RemoveAllListeners();
        upgradeOption1.onClick.AddListener(() => ChooseUpgrade(upgradesToShow[0]));
        upgradeOption2.onClick.RemoveAllListeners();
        upgradeOption2.onClick.AddListener(() => ChooseUpgrade(upgradesToShow[1]));
        upgradeOption3.onClick.RemoveAllListeners();
        upgradeOption3.onClick.AddListener(() => ChooseUpgrade(upgradesToShow[2]));

        Debug.Log("Painel de upgrades mostrado com níveis atualizados.");
    }

    void UpdateUpgradeDescriptions(List<Upgrade> upgradesToShow) {
        if (upgradesToShow.Count > 0) {
            upgradeName1.text = upgradesToShow[0].name;
            upgradeDescription1.text = upgradesToShow[0].description;
            upgradeLevel1.text = $"Nível {upgradeCounts[upgradesToShow[0].name]} / 3";
            upgradeImage1.sprite = upgradesToShow[0].image;
        }

        if (upgradesToShow.Count > 1) {
            upgradeName2.text = upgradesToShow[1].name;
            upgradeDescription2.text = upgradesToShow[1].description;
            upgradeLevel2.text = $"Nível {upgradeCounts[upgradesToShow[1].name]} / 3";
            upgradeImage2.sprite = upgradesToShow[1].image;
        }

        if (upgradesToShow.Count > 2) {
            upgradeName3.text = upgradesToShow[2].name;
            upgradeDescription3.text = upgradesToShow[2].description;
            upgradeLevel3.text = $"Nível {upgradeCounts[upgradesToShow[2].name]} / 3";
            upgradeImage3.sprite = upgradesToShow[2].image;
        }
    }


    public void PopulateSpecialUpgrades() {
        if (!specialUpgradesAdded) {
            availableUpgrades.Clear(); // Limpa os upgrades normais

            Sprite bulletWithPenetrationImage = Resources.Load<Sprite>("Images/bulletWithPenetration");
            Sprite bulletWithKnockbackImage = Resources.Load<Sprite>("Images/bulletWithKnockback");
            Sprite bulletWithCriticalImage = Resources.Load<Sprite>("Images/bulletWithCritical");

            // Adiciona os upgrades especiais usando AddUpgrade para incluir no dicionário upgradeCounts
            AddSpecialUpgrade(new Upgrade("TIRO QUE ATRAVESSA", "Bala atravessa o inimigo", bulletWithPenetrationImage, () => bulletTypeManager.SetBulletType("Penetration")), "TIRO QUE ATRAVESSA");
            AddSpecialUpgrade(new Upgrade("TIRO QUE EMPURRA", "Bala empurra o inimigo para longe", bulletWithKnockbackImage, () => bulletTypeManager.SetBulletType("Knockback")), "TIRO QUE EMPURRA");
            AddSpecialUpgrade(new Upgrade("TIRO CRITICO", "Bala tem chance de critico", bulletWithCriticalImage, () => bulletTypeManager.SetBulletType("Critical")), "TIRO CRITICO");

            specialUpgradesAdded = true; // Marca como adicionados
        }
    }

    private void ChooseUpgrade(Upgrade upgrade) {
        // Desativa os botões para evitar cliques múltiplos
        DisableUpgradeButtons();

        // Aplica o efeito do upgrade
        upgrade.Apply();
        Debug.Log("Upgrade aplicado: " + upgrade.name);

        // Incrementa o nível do upgrade no dicionário upgradeCounts
        if (upgradeCounts.ContainsKey(upgrade.name)) {
            upgradeCounts[upgrade.name]++;
        }
        else {
            upgradeCounts[upgrade.name] = 1;
        }
        Debug.Log("Nível do upgrade atualizado para: " + upgradeCounts[upgrade.name]);

        // Atualiza a descrição dos upgrades para mostrar o novo nível
        UpdateUpgradeDescriptions(new List<Upgrade> { upgrade });

        // Remove upgrade da lista se o limite de nível for alcançado
        if (upgradeCounts[upgrade.name] >= 3) {
            availableUpgrades.Remove(upgrade);
        }

        // Caso especial para o upgrade de Dash
        if (upgrade == dashUpgrade) {
            dashUnlocked = true;
            availableUpgrades.Remove(dashUpgrade);
        }

        // Inicia a Coroutine para fechar o painel após uma pequena espera
        StartCoroutine(CloseUpgradePanelAfterDelay());
    }

    private void DisableUpgradeButtons() {
        upgradeOption1.interactable = false;
        upgradeOption2.interactable = false;
        upgradeOption3.interactable = false;
    }

    private void EnableUpgradeButtons() {
        upgradeOption1.interactable = true;
        upgradeOption2.interactable = true;
        upgradeOption3.interactable = true;
    }

    private IEnumerator CloseUpgradePanelAfterDelay() {
        yield return new WaitForSecondsRealtime(0.25f); // Aguarda meio segundo sem ser afetado pelo Time.timeScale
        Debug.Log("Fechando painel de upgrade...");

        upgradePanel.SetActive(false);
        backgroundOverlay.gameObject.SetActive(false);
        Time.timeScale = 1; // Retorna a escala do tempo ao normal
        Debug.Log("Painel de upgrade fechado e Time.timeScale restaurado");

        // Reativa os botões para a próxima vez
        EnableUpgradeButtons();

        // Inicia a próxima wave
        waveManager.StartNextWaveManual();
    }

    public void ResetToDefaultUpgrades() {
        // Adiciona os upgrades padrões apenas se eles ainda não estiverem presentes na lista
        foreach (var upgrade in availableUpgrades) {
            if (!upgradeCounts.ContainsKey(upgrade.name)) {
                upgradeCounts[upgrade.name] = 0; // Inicializa o nível no dicionário, se não estiver presente
            }
        }
        Debug.Log("Upgrades padrão verificados e atualizados.");
    }

    // Método para habilitar o dash no jogador
    void EnableDash() {
        var playerDash = FindObjectOfType<PlayerDashAfterImage>();
        var playerMovement = FindObjectOfType<PlayerMovement>();

        if (playerDash != null && playerMovement != null) {
            playerDash.enabled = true;
            playerMovement.UnlockDash();
        }
        Debug.Log("Dash habilitado");
    }

    // Método para reduzir o cooldown do dash
    void ReduceDashCooldown(float amount) {
        var playerDash = FindObjectOfType<PlayerDashAfterImage>();
        if (playerDash != null) {
            playerDash.dashCooldown = Mathf.Max(0.2f, playerDash.dashCooldown - amount);
            Debug.Log("Novo cooldown do dash: " + playerDash.dashCooldown);
        }
    }
}

[System.Serializable]
public class Upgrade {
    public string name;
    public string description;
    public Sprite image; // Adiciona a imagem como Sprite
    public System.Action effect;

    public Upgrade(string name, string description, Sprite image, System.Action effect) {
        this.name = name;
        this.description = description;
        this.image = image;
        this.effect = effect;
    }

    public void Apply() {
        effect.Invoke();
        Debug.Log("Upgrade aplicado: " + name);
    }
}

