using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject normalBulletPrefab;
    public GameObject knockbackBulletPrefab;
    public GameObject piercingBulletPrefab;

    private GameObject currentBulletPrefab;

    private void Start() {
        // Defina o tiro inicial
        currentBulletPrefab = normalBulletPrefab;
    }

    /*

    private void Update() {
        /*if (Input.GetButtonDown("Fire1")) {
            Shoot();
        }
    }

    private void Shoot() {
        // Instancia a bullet atual na posição do player
        Instantiate(currentBulletPrefab, transform.position, transform.rotation);
    }

    */
    

    public void ChangeBulletType(int bulletType) {
        switch (bulletType) {
            case 1:
                currentBulletPrefab = normalBulletPrefab;
                break;
            case 2:
                currentBulletPrefab = knockbackBulletPrefab;
                break;
            case 3:
                currentBulletPrefab = piercingBulletPrefab;
                break;
        }
    }
}
