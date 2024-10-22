using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Weapon weapon;
    public float fireRate = 0.2f; // Taxa de disparo (intervalo entre os tiros)
    private float nextFireTime = 0f; // Tempo até o próximo disparo

    Vector2 moveDirection;
    Vector2 mousePosition;

    private PlayerDashAfterImage playerDash; // Referência ao script de dash
    private bool dashUnlocked = false; // Flag para habilitar o dash

    void Start() {
        playerDash = GetComponent<PlayerDashAfterImage>(); // Obtém o script de dash
    }

    void Update() {
        // Movimentação
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Verifica se o botão esquerdo está pressionado e se o cooldown de disparo acabou
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime) {
            weapon.Fire();
            nextFireTime = Time.time + fireRate; // Atualiza o tempo do próximo disparo
        }

        // Define a direção do movimento
        moveDirection = new Vector2(moveX, moveY).normalized;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Executa o dash apenas se estiver desbloqueado
        if (dashUnlocked) {
            playerDash.HandleDash(moveDirection);
        }
    }

    private void FixedUpdate() {
        // Usa GetDashVelocity somente se o dash estiver desbloqueado
        if (dashUnlocked) {
            rb.velocity = playerDash.GetDashVelocity(moveSpeed);
        }
        else {
            rb.velocity = moveDirection * moveSpeed;
        }

        // Rotação do player para mirar na direção do mouse
        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }

    // Método para aumentar a velocidade do player
    // Método para habilitar o dash via UpgradeManager
    public void UnlockDash() {
        dashUnlocked = true;
    }

    // Método para aumentar a velocidade do player
    public void IncreaseMoveSpeed(float amount) {
        moveSpeed += amount;
        Debug.Log("Nova velocidade de movimento: " + moveSpeed);
    }

    public void IncreaseFireRate(float amount) {
        fireRate = Mathf.Max(0.1f, fireRate - amount); // Garante que a taxa de disparo não fique menor que 0.1
        Debug.Log("Nova taxa de disparo: " + fireRate);
    }
}
