using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecialUpgradeManager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;
    private Weapon playerWeapon;

    void Start() {
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        playerWeapon = FindObjectOfType<PlayerMovement>().GetComponentInChildren<Weapon>();
    }

    public void SetSpecialUpgrades(Button option1, TextMeshPro description1, Button option2, TextMeshPro description2, Button option3, TextMeshPro description3) {
        option1.onClick.RemoveAllListeners();
        option2.onClick.RemoveAllListeners();
        option3.onClick.RemoveAllListeners();

        // Definindo as ações para os botões
        option1.onClick.AddListener(ApplySpecialUpgrade1);
        option2.onClick.AddListener(ApplySpecialUpgrade2);
        option3.onClick.AddListener(ApplySpecialUpgrade3);

        // Definindo as descrições dos upgrades
        description1.text = "Upgrade Especial 1: Tiro mais poderoso com dano extra.";
        description2.text = "Upgrade Especial 2: Aumenta a velocidade de ataque em 50%.";
        description3.text = "Upgrade Especial 3: Gera um escudo temporário ao receber dano.";
    }

    void ApplySpecialUpgrade1() {
        // Lógica para o upgrade especial 1
        Debug.Log("Upgrade Especial 1 aplicado!");
    }

    void ApplySpecialUpgrade2() {
        // Lógica para o upgrade especial 2
        Debug.Log("Upgrade Especial 2 aplicado!");
    }

    void ApplySpecialUpgrade3() {
        // Lógica para o upgrade especial 3
        Debug.Log("Upgrade Especial 3 aplicado!");
    }
}
