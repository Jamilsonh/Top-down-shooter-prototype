using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Vida m�xima do player
    public int currentHealth; // Vida atual do player
    public Slider healthBar; // Refer�ncia para o Slider da barra de vida
    public TextMeshProUGUI healthText; // Use essa linha se estiver usando TextMeshPro

    void Start() {
        currentHealth = maxHealth; // Inicializar com a vida m�xima
        UpdateHealthUI();
    }

    // M�todo para receber dano
    public void TakeDamage(int damage) {
        currentHealth -= damage; // Subtrai a quantidade de dano
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Garante que a vida n�o seja negativa
        UpdateHealthUI();

        // Verifica se a vida chegou a zero
        if (currentHealth <= 0) {
            Die(); // Chama o m�todo de morte
        }
    }

    // M�todo para atualizar a barra e o texto de vida
    void UpdateHealthUI() {
        if (healthBar != null) {
            healthBar.value = (float)currentHealth / maxHealth; // Atualiza o valor da barra
        }

        if (healthText != null) {
            healthText.text = currentHealth + "/" + maxHealth; // Atualiza o texto de vida
        }
    }

    // M�todo de morte do player
    void Die() {
        Debug.Log("Player morreu!");
        // Adicione aqui o que deve acontecer quando o player morrer (desativar controle, respawn, etc)
    }

    // Detecta colis�o com inimigos
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) // Verifica se o objeto colidido tem a tag "enemy"
        {
            TakeDamage(10); // Altera o valor do dano conforme necess�rio
        }
    }

    // M�todo para aumentar a vida m�xima e a vida atual do player
    public void IncreaseMaxHealth(int amount) {
        maxHealth += amount; // Aumenta a vida m�xima
        currentHealth += amount; // Aumenta a vida atual para manter a propor��o
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Garante que a vida atual n�o ultrapasse o m�ximo
        UpdateHealthUI(); // Atualiza a UI de vida
    }
}
