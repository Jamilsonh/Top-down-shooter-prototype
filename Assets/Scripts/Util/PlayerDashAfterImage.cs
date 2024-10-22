using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashAfterImage : MonoBehaviour
{
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool isDashing = false;
    private float dashTime = 0f;
    private float lastDashTime = -10f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;

    public GameObject afterimagePrefab; // Prefab da imagem fantasma
    public float afterimageDelay = 0.01f; // Intervalo entre cada imagem fantasma
    private float afterimageTime = 0f;

    // Animation curve para controlar a aceleração/desaceleração
    public AnimationCurve dashCurve;

    void Start() {
        rb = GetComponent<Rigidbody2D>();

        // Exemplo de curva padrão (você pode ajustar no Inspector do Unity)
        dashCurve = new AnimationCurve(
            new Keyframe(0, 1f),    // Começa com velocidade máxima
            new Keyframe(0.5f, 1.5f), // Acelera rápido no começo
            new Keyframe(1, 0.5f)   // Desacelera no final
        );

        // Desativa o script para evitar o dash no início do jogo
        this.enabled = false;
    }

    public void HandleDash(Vector2 currentMoveDirection) {
        moveDirection = currentMoveDirection;

        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDashTime + dashCooldown) {
            isDashing = true;
            dashTime = dashDuration;
            lastDashTime = Time.time;
        }

        if (isDashing) {
            dashTime -= Time.deltaTime;
            afterimageTime -= Time.deltaTime;

            // Cria a imagem fantasma enquanto o dash acontece
            if (afterimageTime <= 0) {
                CreateAfterimage();
                afterimageTime = afterimageDelay; // Reseta o tempo para a próxima imagem fantasma
            }

            if (dashTime <= 0) {
                isDashing = false;
            }
        }
    }

    void CreateAfterimage() {
        // Instancia uma imagem fantasma no local atual do player
        GameObject afterimage = Instantiate(afterimagePrefab, transform.position, transform.rotation);
        // Destroi a imagem fantasma após 0.5 segundos (ajuste conforme necessário)
        Destroy(afterimage, 0.2f);
    }

    public Vector2 GetDashVelocity(float moveSpeed) {
        if (isDashing) {
            // Calcula o progresso do dash baseado no tempo
            float dashProgress = 1f - (dashTime / dashDuration);

            // Usa a curva de aceleração/desaceleração para ajustar a velocidade
            float adjustedSpeed = dashSpeed * dashCurve.Evaluate(dashProgress);
            return moveDirection * adjustedSpeed;
        }

        return moveDirection * moveSpeed;
    }
}
