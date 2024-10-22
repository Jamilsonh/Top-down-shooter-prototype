using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackBullet : MonoBehaviour {
    public KnockbackBulletStats bulletStats; // Referência aos atributos da bala de knockback
    private Transform player; // Referência ao player

    private void Start() {
        // Obtém a referência ao player
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            // Aplica o dano ao inimigo
            EnemyHealthTest enemyHealth = collision.gameObject.GetComponent<EnemyHealthTest>();
            if (enemyHealth != null) {
                enemyHealth.TakeDamage(bulletStats.baseDamage); // Aplica o dano baseado no BulletStats
            }

            // Aplica o efeito de knockback no inimigo, se ele ainda estiver vivo
            if (enemyHealth != null && enemyHealth.health > 0) {
                Rigidbody2D enemyRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (enemyRb != null) {
                    // Calcula a direção do knockback com base na posição do player e do inimigo
                    Vector2 knockbackDirection = (enemyRb.transform.position - player.position).normalized;

                    // Aplica a força de knockback no inimigo
                    enemyRb.AddForce(knockbackDirection * bulletStats.knockbackForce, ForceMode2D.Impulse);

                    // Desativa o movimento do inimigo temporariamente
                    EnemyMovement enemyMovement = collision.gameObject.GetComponent<EnemyMovement>();
                    if (enemyMovement != null) {
                        enemyMovement.DisableMovement(bulletStats.knockbackDuration); // Usa a duração do BulletStats
                    }
                }
            }

            // Destroi o tiro após a colisão
            Destroy(gameObject);
        }
    }
}
