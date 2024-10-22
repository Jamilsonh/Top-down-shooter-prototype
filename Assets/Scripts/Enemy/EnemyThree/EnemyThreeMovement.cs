using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThreeMovement : MonoBehaviour {
    public float normalSpeed = 3f;
    public float dashSpeed = 8f;
    public float pauseDuration = 1f;
    public float detectionRange = 5f;
    public float dashCooldown = 3f;
    private Transform player;
    public bool isDead = false;
    private bool isDashing = false;
    private bool canDash = true;
    private Rigidbody2D rb; // Adicionando o Rigidbody2D
    private Vector2 dashDirection;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>(); // Obtendo o Rigidbody2D
    }

    void Update() {
        if (isDead) return;

        if (!isDashing && canDash) {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= detectionRange) {
                StartCoroutine(DashTowardsPlayer());
            }
            else {
                FollowPlayer();
            }
        }
        else if (!isDashing) {
            FollowPlayer();
        }
    }

    void FollowPlayer() {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * normalSpeed * Time.fixedDeltaTime);
    }

    IEnumerator DashTowardsPlayer() {
        canDash = false;
        isDashing = true;
        yield return new WaitForSeconds(pauseDuration);

        dashDirection = (player.position - transform.position).normalized;
        float dashTime = 0.5f;
        float dashElapsed = 0f;

        while (dashElapsed < dashTime) {
            rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.fixedDeltaTime);
            dashElapsed += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (isDashing && collision.gameObject.CompareTag("Player")) {
            Debug.Log("O inimigo atingiu o player!");
            // Ação adicional ao acertar o player
        }
    }
}
