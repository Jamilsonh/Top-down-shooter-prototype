using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CriticalBullet : MonoBehaviour {
    public CriticalBulletStats bulletStats; // Referência para os atributos da bala crítica
    public GameObject criticalEffect; // Prefab do efeito crítico
    public GameObject criticalTextPrefab; // Prefab do texto de dano crítico
    private bool isCritical = false; // Indica se o tiro é crítico

    private void Start() {
        // Verifica se é um tiro crítico
        isCritical = IsCriticalHit();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            // Calcula o dano final com base se for crítico ou não
            float finalDamage = isCritical ? bulletStats.baseDamage * bulletStats.criticalMultiplier : bulletStats.baseDamage;

            // Aplica o dano ao inimigo
            EnemyHealthTest enemyHealth = collision.gameObject.GetComponent<EnemyHealthTest>();
            if (enemyHealth != null) {
                enemyHealth.TakeDamage(finalDamage);
            }

            // Se for crítico, instancia o efeito de partículas e exibe o texto de dano crítico
            if (isCritical) {
                ShowCriticalVisuals(finalDamage);
            }

            // Destroi o tiro após a colisão
            Destroy(gameObject);
        }
    }

    // Método para verificar se o tiro é crítico
    private bool IsCriticalHit() {
        // Gera um número aleatório entre 0 e 1
        return Random.value < bulletStats.criticalChance;
    }

    // Método para mostrar os efeitos visuais do crítico
    private void ShowCriticalVisuals(float damage) {
        // Instancia o efeito de partículas
        if (criticalEffect != null) {
            Instantiate(criticalEffect, transform.position, Quaternion.identity);
        }

        // Instancia o texto de dano crítico
        if (criticalTextPrefab != null) {
            GameObject criticalText = Instantiate(criticalTextPrefab, transform.position, Quaternion.identity);
            TextMeshProUGUI textComponent = criticalText.GetComponentInChildren<TextMeshProUGUI>();

            if (textComponent != null) {
                textComponent.text = "CRITICAL";
                textComponent.color = Color.red;
            }

            // Definir a posição no mundo perto do ponto de impacto
            criticalText.transform.position = transform.position + new Vector3(0, 1, 0);
        }
    }
}
