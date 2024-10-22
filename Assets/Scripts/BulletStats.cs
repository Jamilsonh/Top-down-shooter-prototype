using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletStats {
    public float baseDamage = 10f;   // Dano base da bala
}

[System.Serializable]
public class CriticalBulletStats : BulletStats {
    public float criticalMultiplier = 2f;   // Multiplicador de dano crítico
    public float criticalChance = 0.2f;     // Chance de acerto crítico
}

[System.Serializable]
public class KnockbackBulletStats : BulletStats {
    public float knockbackForce = 2f; // Força do knockback
    public float knockbackDuration = 1f; // Duração do knockback
}

[System.Serializable]
public class PenetrationBulletStats : BulletStats {
    public int maxEnemiesHit = 2; // Quantidade máxima de inimigos que a bala pode atravessar
}