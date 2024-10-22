using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntennaScript : MonoBehaviour
{
    public float forceAmount = 500f; // Força que será aplicada
    private Rigidbody2D rb;
    public int damageAmount = 10; // Dano causado ao player ao ser atingido

    void Start() {
        rb = GetComponent<Rigidbody2D>();

        // Inicialmente, o Rigidbody é desativado
        rb.simulated = false;
    }

    public void Launch(Vector2 targetPosition) {
        // Desacoplar a antena
        transform.parent = null;

        // Ativar o Rigidbody2D para começar a simulação física
        rb.simulated = true;

        // Calcula a direção em que a antena será lançada
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        // Aplica a força na direção do player
        rb.AddForce(direction * forceAmount);
    }

    // Detecta a colisão com o player
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null) {
                playerHealth.TakeDamage(damageAmount); // Aplica dano ao player
                Destroy(gameObject); // Destroi a antena após a colisão (opcional)
            }
        }
    }
}
