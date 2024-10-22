using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public GameObject bulletPrefab; // Prefab do tiro atual
    public Transform firePoint;     // Ponto de disparo do tiro
    public float fireForce = 20f;   // For�a do tiro

    // M�todo para disparar o tiro
    public void Fire() {
        // Instancia o tiro na posi��o e rota��o do ponto de disparo
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Adiciona uma for�a ao tiro na dire��o do ponto de disparo
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
    }
}
