using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenetrationBullet : MonoBehaviour {
    public PenetrationBulletStats bulletStats; // Referência aos atributos da bala de penetração
    private int enemiesHit = 0; // Contador de inimigos atingidos

    private void OnTriggerEnter2D(Collider2D collision) {
        // Verifica se colidiu com um inimigo
        if (collision.CompareTag("Enemy")) {
            // Aplica o dano ao inimigo
            EnemyHealthTest enemyHealth = collision.GetComponent<EnemyHealthTest>();
            if (enemyHealth != null) {
                enemyHealth.TakeDamage(bulletStats.baseDamage); // Usa o dano do BulletStats
            }

            // Incrementa o contador de inimigos atingidos
            enemiesHit++;

            // Se atingir o número máximo de inimigos, destrói o tiro
            if (enemiesHit >= bulletStats.maxEnemiesHit) {
                Destroy(gameObject);
            }
        }
    }
}
