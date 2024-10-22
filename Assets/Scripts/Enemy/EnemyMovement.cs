using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwoEvolveMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Transform player;
    private Rigidbody2D rb;
    private Vector2 movement;
    public bool isDead = false;
    private bool canMove = true;
    public GameObject antenna1; // Referência para a antena 1
    public GameObject antenna2; // Referência para a antena 2
    public float detectionRange = 5f; // Range em que o inimigo detecta o player
    private bool antenna1Launched = false;
    private bool antenna2Launched = false;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (canMove && !isDead) {
            float distance = Vector2.Distance(transform.position, player.position);

            // Se o player estiver no range e as antenas não foram lançadas
            if (distance <= detectionRange) {
                if (!antenna1Launched) {
                    StartCoroutine(LaunchAntenna(antenna1, 1f)); // Lança a antena 1 após 1 segundo
                    antenna1Launched = true;
                }
                else if (!antenna2Launched) {
                    StartCoroutine(LaunchAntenna(antenna2, 2f)); // Lança a antena 2 após 2 segundos
                    antenna2Launched = true;
                }
            }

            Vector3 direction = player.position - transform.position;
            direction.Normalize();
            movement = direction;
        }
    }

    void FixedUpdate() {
        if (!isDead && canMove) {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    IEnumerator LaunchAntenna(GameObject antenna, float delay) {
        canMove = false; // Pausa o movimento durante o lançamento
        yield return new WaitForSeconds(delay);

        // Desacopla a antena do inimigo
        antenna.transform.parent = null;

        // Ativa o script da antena para aplicar a força
        antenna.GetComponent<AntennaScript>().Launch(player.position);

        canMove = true; // Volta a se mover
    }
}
