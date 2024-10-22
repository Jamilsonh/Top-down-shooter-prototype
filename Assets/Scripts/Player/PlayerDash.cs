using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public float dashSpeed = 10f; // Velocidade do dash
    public float dashDuration = 0.2f; // Duração do dash
    public float dashCooldown = 1f; // Tempo de cooldown entre dashes
    private bool isDashing = false;
    private float dashTime = 0f;
    private float lastDashTime = -10f; // Garantir que o dash possa ser usado no início do jogo

    private Rigidbody2D rb;
    private Vector2 moveDirection;

    void Start() {
        rb = GetComponent<Rigidbody2D>(); // Obtém o Rigidbody2D do player
    }

    public void HandleDash(Vector2 currentMoveDirection) {
        moveDirection = currentMoveDirection;

        // Verifica se o jogador está tentando dar dash
        if (Input.GetKeyDown(KeyCode.T) && Time.time >= lastDashTime + dashCooldown) {
            isDashing = true;
            dashTime = dashDuration;
            lastDashTime = Time.time;
        }

        // Se estiver dashing, usa a velocidade do dash
        if (isDashing) {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0) {
                isDashing = false; // Termina o dash
            }
        }
    }

    public Vector2 GetDashVelocity(float moveSpeed) {
        if (isDashing) {
            return moveDirection * dashSpeed; // Retorna a velocidade do dash
        }
        return moveDirection * moveSpeed; // Retorna a velocidade normal de movimento
    }
}
