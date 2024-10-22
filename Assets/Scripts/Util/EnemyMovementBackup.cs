using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementBackup : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Transform player;
    private Rigidbody2D rb;
    private Vector2 movement;
    public bool isDead = false;
    private bool canMove = true; // Nova variável para controlar o movimento

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (canMove) {
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

    // Função para desativar o movimento temporariamente
    public void DisableMovement(float duration) {
        StartCoroutine(DisableMovementTemporarily(duration));
    }

    IEnumerator DisableMovementTemporarily(float duration) {
        canMove = false; // Desativa o movimento
        yield return new WaitForSeconds(duration); // Espera o tempo especificado
        canMove = true; // Reativa o movimento
    }
}
