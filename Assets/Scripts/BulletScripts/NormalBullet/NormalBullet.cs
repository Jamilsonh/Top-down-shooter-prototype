using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : MonoBehaviour {
    private BulletTypeManager bulletTypeManager; // Referência ao BulletTypeManager

    private void Start() {
        bulletTypeManager = FindObjectOfType<BulletTypeManager>(); // Busca a referência ao BulletTypeManager
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            // Aplica o dano com base no BulletStats atualizado
            EnemyHealthTest enemyHealth = collision.gameObject.GetComponent<EnemyHealthTest>();
            if (enemyHealth != null) {
                float updatedDamage = bulletTypeManager.currentBulletStats.baseDamage;
                enemyHealth.TakeDamage(updatedDamage); // Aplica o dano atualizado
                Debug.Log("Dano aplicado: " + updatedDamage);
            }
            Destroy(gameObject);
        }
    }
}
