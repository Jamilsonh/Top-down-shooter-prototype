using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTwoTest : MonoBehaviour
{
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.5f;
    private Transform player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            Rigidbody2D enemyRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (enemyRb != null) {
                // Direção baseada na diferença entre o inimigo e o player
                Vector2 knockbackDirection = (enemyRb.transform.position - player.position).normalized;

                // Aplicação do knockback suavizado
                Vector2 smoothedKnockback = Vector2.Lerp(enemyRb.velocity, knockbackDirection * knockbackForce, 0.5f);

                enemyRb.AddForce(smoothedKnockback, ForceMode2D.Impulse);

                // Desativar o movimento do inimigo temporariamente
                EnemyMovement enemyMovement = collision.gameObject.GetComponent<EnemyMovement>();
                if (enemyMovement != null) {
                    enemyMovement.DisableMovement(knockbackDuration);
                }
            }
        }

        Destroy(gameObject);
    }
}
