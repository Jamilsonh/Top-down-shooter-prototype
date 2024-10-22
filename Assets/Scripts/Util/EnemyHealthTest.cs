using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthTest : MonoBehaviour
{
    public float health = 20f; // Vida total do inimigo
    public SpriteRenderer spriteRenderer;
    public float flashDuration = 0.1f;
    public GameObject deathEffectPrefab; // O prefab da animação de morte

    private Color originalColor;
    private bool isRegistered = false; // Variável para garantir que o inimigo seja registrado apenas uma vez
    private bool isDead = false; // Variável de controle para parar corrotinas após a morte

    // Referência ao script de movimentação do inimigo
    private EnemyMovement enemyMovementScript;

    void Start() {
        // Salva a cor original do inimigo
        if (spriteRenderer != null) {
            originalColor = spriteRenderer.color;
        }

        // Pega a referência do script de movimentação
        enemyMovementScript = GetComponent<EnemyMovement>();

        // Registra o inimigo na WaveManager
        if (!isRegistered) {
            FindObjectOfType<WaveManager>().RegisterEnemy(gameObject);
            isRegistered = true;
        }
    }

    public void TakeDamage(float damageAmount) {
        if (isDead) return; // Não aplica dano se o inimigo já está morto

        health -= damageAmount; // Reduz a vida do inimigo pelo valor do dano

        // Inicia a animação de piscar
        if (health > 0) {
            StartCoroutine(FlashSprite());
        }

        // Se a vida chegar a 0 ou menos, o inimigo morre
        if (health <= 0) {
            Die();
        }
    }

    public IEnumerator FlashSprite() {
        if (spriteRenderer != null && !isDead) {
            spriteRenderer.color = Color.white; // Pisca em branco ao ser atingido
            yield return new WaitForSeconds(flashDuration);

            if (!isDead) { // Verifica se o inimigo ainda está vivo
                spriteRenderer.color = originalColor;
            }
        }
    }

    public void Die() {
        // Sinaliza que o inimigo está morto
        isDead = true;

        // Sinaliza para o script de movimentação que o inimigo está morto
        if (enemyMovementScript != null) {
            enemyMovementScript.isDead = true;
        }

        // Notifica o WaveManager que o inimigo morreu
        FindObjectOfType<WaveManager>().UnregisterEnemy(gameObject);

        // Chama o efeito de morte instantaneamente
        SpawnDeathEffect();

        // Destroi o inimigo após o efeito de morte
        Destroy(gameObject);
    }

    private void SpawnDeathEffect() {
        // Instancia a animação de efeito de morte na posição do inimigo
        if (deathEffectPrefab != null) {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }
    }
}

