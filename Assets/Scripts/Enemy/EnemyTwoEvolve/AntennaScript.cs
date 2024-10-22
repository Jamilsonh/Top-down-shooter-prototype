using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntennaScript : MonoBehaviour
{
    public float forceAmount = 500f; // For�a que ser� aplicada
    private Rigidbody2D rb;
    public int damageAmount = 10; // Dano causado ao player ao ser atingido

    void Start() {
        rb = GetComponent<Rigidbody2D>();

        // Inicialmente, o Rigidbody � desativado
        rb.simulated = false;
    }

    public void Launch(Vector2 targetPosition) {
        // Desacoplar a antena
        transform.parent = null;

        // Ativar o Rigidbody2D para come�ar a simula��o f�sica
        rb.simulated = true;

        // Calcula a dire��o em que a antena ser� lan�ada
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        // Aplica a for�a na dire��o do player
        rb.AddForce(direction * forceAmount);
    }

    // Detecta a colis�o com o player
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null) {
                playerHealth.TakeDamage(damageAmount); // Aplica dano ao player
                Destroy(gameObject); // Destroi a antena ap�s a colis�o (opcional)
            }
        }
    }
}
