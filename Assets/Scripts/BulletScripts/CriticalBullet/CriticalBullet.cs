using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CriticalBullet : MonoBehaviour {
    public CriticalBulletStats bulletStats; // Refer�ncia para os atributos da bala cr�tica
    public GameObject criticalEffect; // Prefab do efeito cr�tico
    public GameObject criticalTextPrefab; // Prefab do texto de dano cr�tico
    private bool isCritical = false; // Indica se o tiro � cr�tico

    private void Start() {
        // Verifica se � um tiro cr�tico
        isCritical = IsCriticalHit();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            // Calcula o dano final com base se for cr�tico ou n�o
            float finalDamage = isCritical ? bulletStats.baseDamage * bulletStats.criticalMultiplier : bulletStats.baseDamage;

            // Aplica o dano ao inimigo
            EnemyHealthTest enemyHealth = collision.gameObject.GetComponent<EnemyHealthTest>();
            if (enemyHealth != null) {
                enemyHealth.TakeDamage(finalDamage);
            }

            // Se for cr�tico, instancia o efeito de part�culas e exibe o texto de dano cr�tico
            if (isCritical) {
                ShowCriticalVisuals(finalDamage);
            }

            // Destroi o tiro ap�s a colis�o
            Destroy(gameObject);
        }
    }

    // M�todo para verificar se o tiro � cr�tico
    private bool IsCriticalHit() {
        // Gera um n�mero aleat�rio entre 0 e 1
        return Random.value < bulletStats.criticalChance;
    }

    // M�todo para mostrar os efeitos visuais do cr�tico
    private void ShowCriticalVisuals(float damage) {
        // Instancia o efeito de part�culas
        if (criticalEffect != null) {
            Instantiate(criticalEffect, transform.position, Quaternion.identity);
        }

        // Instancia o texto de dano cr�tico
        if (criticalTextPrefab != null) {
            GameObject criticalText = Instantiate(criticalTextPrefab, transform.position, Quaternion.identity);
            TextMeshProUGUI textComponent = criticalText.GetComponentInChildren<TextMeshProUGUI>();

            if (textComponent != null) {
                textComponent.text = "CRITICAL";
                textComponent.color = Color.red;
            }

            // Definir a posi��o no mundo perto do ponto de impacto
            criticalText.transform.position = transform.position + new Vector3(0, 1, 0);
        }
    }
}
