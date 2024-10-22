using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {
    public int currentWave = 1;
    public int baseEnemyCount = 10;
    public int baseEnemyTwoCount = 3;
    public int baseEnemyThreeCount = 2; // Quantidade base do terceiro inimigo
    public float timeBetweenWaves = 5f;
    public float upgradeDelay = 2f; // Tempo de espera antes de mostrar os upgrades

    private EnemySpawn enemySpawner;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private UpgradeManager upgradeManager;

    void Start() {
        enemySpawner = GetComponent<EnemySpawn>();
        upgradeManager = FindObjectOfType<UpgradeManager>();
        StartCoroutine(StartNextWave()); // Inicia a primeira wave
    }

    public IEnumerator StartNextWave() {
        Debug.Log("Iniciando a próxima wave em " + timeBetweenWaves + " segundos.");
        yield return new WaitForSeconds(timeBetweenWaves);
        StartWave();
    }

    public void StartNextWaveManual() {
        StartCoroutine(StartNextWave());
    }

    void StartWave() {
        Debug.Log("Iniciando a Wave: " + currentWave);
        int enemyCount = baseEnemyCount * currentWave;
        int enemyTwoCount = 0;
        int enemyThreeCount = 0;

        if (currentWave >= 3) {
            enemyTwoCount = baseEnemyTwoCount + (currentWave - 3);
        }

        if (currentWave >= 5) {
            enemyThreeCount = baseEnemyThreeCount + (currentWave - 5);
        }

        StartCoroutine(SpawnWaveEnemies(enemyCount, enemyTwoCount, enemyThreeCount));
    }

    private IEnumerator SpawnWaveEnemies(int enemyCount, int enemyTwoCount, int enemyThreeCount) {
        while (enemyCount > 0 || enemyTwoCount > 0 || enemyThreeCount > 0) {
            // Cria uma lista de tipos de inimigos disponíveis para spawnar
            List<int> availableEnemyTypes = new List<int>();
            if (enemyCount > 0) availableEnemyTypes.Add(1);
            if (enemyTwoCount > 0) availableEnemyTypes.Add(2);
            if (enemyThreeCount > 0) availableEnemyTypes.Add(3);

            // Seleciona aleatoriamente um tipo de inimigo da lista disponível
            int selectedEnemyType = availableEnemyTypes[Random.Range(0, availableEnemyTypes.Count)];

            int enemiesToSpawn = 0;

            // Definindo o número de inimigos a serem spawnados com base na wave atual
            switch (selectedEnemyType) {
                case 1:
                    enemiesToSpawn = Mathf.Min(enemyCount, GetEnemySpawnRange(1)); // Usa uma função para determinar o range
                    enemyCount -= enemiesToSpawn;
                    break;
                case 2:
                    enemiesToSpawn = Mathf.Min(enemyTwoCount, GetEnemySpawnRange(2));
                    enemyTwoCount -= enemiesToSpawn;
                    break;
                case 3:
                    enemiesToSpawn = Mathf.Min(enemyThreeCount, GetEnemySpawnRange(3));
                    enemyThreeCount -= enemiesToSpawn;
                    break;
            }

            // Spawna os inimigos usando sua lógica existente
            if (currentWave >= 3) {
                SpawnEnemyWithMultiRegion(enemiesToSpawn, selectedEnemyType);
            }
            else {
                enemySpawner.SpawnEnemyGroup(enemiesToSpawn, selectedEnemyType);
            }

            yield return new WaitForSeconds(enemySpawner.spawnRate);
        }
    }

    // Função para determinar o range de spawn com base no tipo de inimigo e na wave atual
    private int GetEnemySpawnRange(int enemyType) {
        if (currentWave >= 1 && currentWave <= 3) {
            // Para waves de 1 a 3
            if (enemyType == 1) return Random.Range(1, 5);  // Inimigo 1 spawna entre 1 e 5
            if (enemyType == 2) return Random.Range(1, 3);  // Inimigo 2 spawna entre 1 e 3
            if (enemyType == 3) return Random.Range(1, 2);  // Inimigo 3 spawna entre 1 e 2
        }
        else if (currentWave >= 4 && currentWave <= 7) {
            // Para waves de 4 a 7
            if (enemyType == 1) return Random.Range(3, 10);  // Inimigo 1 spawna entre 3 e 10
            if (enemyType == 2) return Random.Range(2, 5);   // Inimigo 2 spawna entre 2 e 5
            if (enemyType == 3) return Random.Range(1, 3);   // Inimigo 3 spawna entre 1 e 3
        }
        else if (currentWave >= 8) {
            // Para waves de 8 em diante
            if (enemyType == 1) return Random.Range(5, 15);  // Inimigo 1 spawna entre 5 e 15
            if (enemyType == 2) return Random.Range(3, 6);   // Inimigo 2 spawna entre 3 e 6
            if (enemyType == 3) return Random.Range(2, 4);   // Inimigo 3 spawna entre 2 e 4
        }

        // Valor padrão se nenhuma condição for atendida
        return Random.Range(1, 4);
    }

    // Método para spawnar inimigos por dois lados ao mesmo tempo
    private void SpawnEnemyWithMultiRegion(int enemyCount, int enemyType) {
        // Escolhe duas regiões diferentes aleatoriamente
        int firstRegion = Random.Range(0, 4);
        int secondRegion;
        do {
            secondRegion = Random.Range(0, 4);
        } while (secondRegion == firstRegion); // Garante que as duas regiões são diferentes

        // Spawna inimigos na primeira região
        enemySpawner.SpawnEnemyGroup(enemyCount / 2, enemyType); // Metade dos inimigos na primeira região

        // Spawna inimigos na segunda região
        enemySpawner.SpawnEnemyGroup(enemyCount - enemyCount / 2, enemyType); // Metade restante na segunda região
    }

    public void RegisterEnemy(GameObject enemy) {
        if (!activeEnemies.Contains(enemy)) {
            activeEnemies.Add(enemy);
            Debug.Log("Inimigo registrado. Total de inimigos vivos: " + activeEnemies.Count);
        }
        else {
            Debug.Log("Inimigo já está registrado, não será adicionado novamente.");
        }
    }

    public void UnregisterEnemy(GameObject enemy) {
        if (activeEnemies.Contains(enemy)) {
            activeEnemies.Remove(enemy);
            Debug.Log("Inimigo removido. Total de inimigos vivos: " + activeEnemies.Count);

            if (activeEnemies.Count == 0) {
                Debug.Log("Todos os inimigos mortos. Exibindo upgrades após atraso.");
                currentWave++;
                StartCoroutine(ShowUpgradeWithDelay()); // Inicia a Coroutine para mostrar os upgrades com atraso
            }
        }
        else {
            Debug.Log("Inimigo já foi removido.");
        }
    }

    // Mostra os upgrades após um atraso
    private IEnumerator ShowUpgradeWithDelay() {
        yield return new WaitForSeconds(upgradeDelay); // Espera antes de mostrar os upgrades

        // Mostra o painel de upgrades baseado na wave atual
        upgradeManager.ShowUpgrades();
    }
}
