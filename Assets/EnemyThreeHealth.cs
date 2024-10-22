using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThreeHealth : MonoBehaviour
{
    public int health = 2; // Número de tiros para matar o inimigo
    public SpriteRenderer spriteRenderer;
    public float flashDuration = 0.1f;
    public GameObject deathEffectPrefab; // O prefab da animação de morte

    private Color originalColor;

    // Referência ao script de movimentação do inimigo
    private EnemyThreeMovement enemyMovementScript;

    void Start() {
        // Salva a cor original e pega o Animator do inimigo
        originalColor = spriteRenderer.color;

        // Pega a referência do script de movimentação
        enemyMovementScript = GetComponent<EnemyThreeMovement>(); // Nome do script de movimentação
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Bullet")) {
            health--;

            Destroy(collision.gameObject); // Destroi o tiro

            // Inicia a animação de piscar
            StartCoroutine(FlashSprite());

            // Se a vida chegar a 0, o inimigo morre
            if (health <= 0) {
                Die();
            }
        }
    }

    private IEnumerator FlashSprite() {
        spriteRenderer.color = Color.white; // Pisca em branco
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    private void Die() {
        // Sinaliza para o script de movimentação que o inimigo está morto
        enemyMovementScript.isDead = true;

        // Chama o efeito de morte instantaneamente
        SpawnDeathEffect();
    }

    private void SpawnDeathEffect() {
        // Instancia a animação de efeito de morte na posição do inimigo
        Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);

        // Destroi o inimigo após o efeito
        Destroy(gameObject);
    }
}
