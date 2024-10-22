using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject enemyTwoPrefab; // Prefab do segundo inimigo
    public GameObject enemyThreePrefab; // Prefab do terceiro inimigo
    public float spawnRate = 0.5f; // Taxa de spawn entre inimigos de uma mesma wave
    public float spawnDistanceFromEdge = 1f;
    public BoxCollider2D mapBounds;
    public float spawnSpread = 2f; // Varia��o de posi��o para distribuir os inimigos

    private float leftBound;
    private float rightBound;
    private float topBound;
    private float bottomBound;

    private void Start() {
        // Calcula os limites do mapa usando o BoxCollider2D
        if (mapBounds != null) {
            leftBound = mapBounds.bounds.min.x;
            rightBound = mapBounds.bounds.max.x;
            topBound = mapBounds.bounds.max.y;
            bottomBound = mapBounds.bounds.min.y;
        }
    }

    public void SpawnEnemies(int count, int enemyType) {
        StartCoroutine(SpawnEnemiesRoutine(count, enemyType));
    }

    private IEnumerator SpawnEnemiesRoutine(int count, int enemyType) {
        for (int i = 0; i < count; i++) {
            SpawnEnemyGroup(1, enemyType); // Passa o tipo de inimigo como inteiro
            yield return new WaitForSeconds(spawnRate);
        }
    }

    public void SpawnEnemyGroup(int enemyCount, int enemyType) {
        // Define uma das quatro regi�es aleatoriamente
        int region = Random.Range(0, 4);
        Vector2 baseSpawnPosition = Vector2.zero;

        // Vari�vel para definir se vai spawnar em linha ou de forma tradicional
        bool spawnInLine = Random.value > 0.5f; // 50% de chance de spawnar em linha

        // Define a varia��o ao longo do eixo perpendicular se for spawn em linha
        Vector2 spawnVariation = Vector2.zero;

        // Determina a posi��o base de spawn dependendo da regi�o
        switch (region) {
            case 0: // Esquerda
                baseSpawnPosition = new Vector2(leftBound - spawnDistanceFromEdge, Random.Range(bottomBound, topBound));
                spawnVariation = new Vector2(0, spawnSpread); // Variar somente no eixo Y se for spawn em linha
                break;
            case 1: // Direita
                baseSpawnPosition = new Vector2(rightBound + spawnDistanceFromEdge, Random.Range(bottomBound, topBound));
                spawnVariation = new Vector2(0, spawnSpread); // Variar somente no eixo Y se for spawn em linha
                break;
            case 2: // Topo
                baseSpawnPosition = new Vector2(Random.Range(leftBound, rightBound), topBound + spawnDistanceFromEdge);
                spawnVariation = new Vector2(spawnSpread, 0); // Variar somente no eixo X se for spawn em linha
                break;
            case 3: // Baixo
                baseSpawnPosition = new Vector2(Random.Range(leftBound, rightBound), bottomBound - spawnDistanceFromEdge);
                spawnVariation = new Vector2(spawnSpread, 0); // Variar somente no eixo X se for spawn em linha
                break;
        }

        // Spawna os inimigos em torno da posi��o base com varia��o dependendo da l�gica escolhida
        for (int i = 0; i < enemyCount; i++) {
            Vector2 spawnPosition;

            if (spawnInLine) {
                // Spawn em linha (varia��o em apenas um eixo)
                spawnPosition = baseSpawnPosition + spawnVariation * i;
            }
            else {
                // Spawn tradicional (varia��o em ambos os eixos)
                spawnPosition = baseSpawnPosition + new Vector2(Random.Range(-spawnSpread, spawnSpread), Random.Range(-spawnSpread, spawnSpread));
            }

            GameObject enemy;

            // Verifica o tipo de inimigo
            switch (enemyType) {
                case 1:
                    enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                    break;
                case 2:
                    enemy = Instantiate(enemyTwoPrefab, spawnPosition, Quaternion.identity);
                    break;
                case 3:
                    enemy = Instantiate(enemyThreePrefab, spawnPosition, Quaternion.identity);
                    break;
                default:
                    enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                    break;
            }

            // Registra o inimigo no WaveManager
            FindObjectOfType<WaveManager>().RegisterEnemy(enemy);
            Debug.Log("Inimigo spawnado na wave " + FindObjectOfType<WaveManager>().currentWave + " - Tipo: " + enemyType);
        }
    }
}
