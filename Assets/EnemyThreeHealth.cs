using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThreeHealth : MonoBehaviour
{
    public int health = 2; // N�mero de tiros para matar o inimigo
    public SpriteRenderer spriteRenderer;
    public float flashDuration = 0.1f;
    public GameObject deathEffectPrefab; // O prefab da anima��o de morte

    private Color originalColor;

    // Refer�ncia ao script de movimenta��o do inimigo
    private EnemyThreeMovement enemyMovementScript;

    void Start() {
        // Salva a cor original e pega o Animator do inimigo
        originalColor = spriteRenderer.color;

        // Pega a refer�ncia do script de movimenta��o
        enemyMovementScript = GetComponent<EnemyThreeMovement>(); // Nome do script de movimenta��o
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Bullet")) {
            health--;

            Destroy(collision.gameObject); // Destroi o tiro

            // Inicia a anima��o de piscar
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
        // Sinaliza para o script de movimenta��o que o inimigo est� morto
        enemyMovementScript.isDead = true;

        // Chama o efeito de morte instantaneamente
        SpawnDeathEffect();
    }

    private void SpawnDeathEffect() {
        // Instancia a anima��o de efeito de morte na posi��o do inimigo
        Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);

        // Destroi o inimigo ap�s o efeito
        Destroy(gameObject);
    }
}
