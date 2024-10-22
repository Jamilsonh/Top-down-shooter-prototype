using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public GameObject bulletPrefab; // Prefab do tiro atual
    public Transform firePoint;     // Ponto de disparo do tiro
    public float fireForce = 20f;   // Força do tiro

    // Método para disparar o tiro
    public void Fire() {
        // Instancia o tiro na posição e rotação do ponto de disparo
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Adiciona uma força ao tiro na direção do ponto de disparo
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
    }
}
