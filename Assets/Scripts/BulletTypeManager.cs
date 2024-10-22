using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTypeManager : MonoBehaviour
{
    public BulletStats currentBulletStats; // Atributos da bala atual

    public GameObject penetrationBulletPrefab;
    public GameObject knockbackBulletPrefab;
    public GameObject criticalBulletPrefab;

    private Weapon playerWeapon; // Referência para o script Weapon do player

    void Start() {
        // Encontra o Weapon no jogador (certifique-se de que o objeto do player tem o script Weapon)
        playerWeapon = FindObjectOfType<Weapon>();
    }

    public void SetBulletType(string bulletType) {
        switch (bulletType) {
            case "Penetration":
                currentBulletStats = penetrationBulletPrefab.GetComponent<BulletData>().bulletStats;
                playerWeapon.bulletPrefab = penetrationBulletPrefab; // Atualiza o prefab no Weapon
                break;
            case "Knockback":
                currentBulletStats = knockbackBulletPrefab.GetComponent<BulletData>().bulletStats;
                playerWeapon.bulletPrefab = knockbackBulletPrefab; // Atualiza o prefab no Weapon
                break;
            case "Critical":
                currentBulletStats = criticalBulletPrefab.GetComponent<BulletData>().bulletStats;
                playerWeapon.bulletPrefab = criticalBulletPrefab; // Atualiza o prefab no Weapon
                break;
        }
        Debug.Log("Tipo de bala atualizado: " + bulletType);
        Debug.Log("Dano atual da bala: " + currentBulletStats.baseDamage);
    }

    public void IncreaseBulletDamage(float amount) {
        if (currentBulletStats != null) {
            currentBulletStats.baseDamage += amount; // Atualiza o valor do dano diretamente no BulletStats
            Debug.Log("Dano atualizado para: " + currentBulletStats.baseDamage);
        }
        else {
            Debug.LogError("currentBulletStats está nulo.");
        }
    }
}
