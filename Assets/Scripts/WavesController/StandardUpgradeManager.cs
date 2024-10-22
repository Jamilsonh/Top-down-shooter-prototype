using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StandardUpgradeManager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;
    private Weapon playerWeapon;

    public GameObject knockbackBulletPrefab; // Prefab da knockback bullet para o upgrade

    void Start() {
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        playerWeapon = FindObjectOfType<PlayerMovement>().GetComponentInChildren<Weapon>();
    }

    public void SetStandardUpgrades(Button option1, TextMeshPro description1, Button option2, TextMeshPro description2, Button option3, TextMeshPro description3) {
        option1.onClick.RemoveAllListeners();
        option2.onClick.RemoveAllListeners();
        option3.onClick.RemoveAllListeners();

        // Definindo as ações para os botões
        option1.onClick.AddListener(ApplyStandardUpgrade1);
        option2.onClick.AddListener(ApplyStandardUpgrade2);
        option3.onClick.AddListener(ApplyStandardUpgrade3);

        // Definindo as descrições dos upgrades
        description1.text = "Muda o tiro para Knockback Bullet, empurrando inimigos.";
        description2.text = "Aumenta a vida máxima do player em 20.";
        description3.text = "Aumenta a velocidade de movimento do player em 2.";
    }

    void ApplyStandardUpgrade1() {
        if (playerWeapon != null) {
            playerWeapon.bulletPrefab = knockbackBulletPrefab; // Altera o bulletPrefab para knockback
            Debug.Log("Upgrade Padrão 1 aplicado! Tiro alterado para Knockback Bullet.");
        }
    }

    void ApplyStandardUpgrade2() {
        if (playerHealth != null) {
            int healthIncrease = 20; // Quantidade de vida que você deseja aumentar
            playerHealth.IncreaseMaxHealth(healthIncrease);
            Debug.Log("Upgrade Padrão 2 aplicado! Vida aumentada em " + healthIncrease);
        }
    }

    void ApplyStandardUpgrade3() {
        playerMovement.IncreaseMoveSpeed(2f);
        Debug.Log("Upgrade Padrão 3 aplicado! Velocidade aumentada.");
    }
}
