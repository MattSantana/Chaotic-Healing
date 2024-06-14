using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs; // Array de prefabs de inimigos para escolher aleatoriamente
    [SerializeField] private Transform[] spawnPoints; // Pontos de spawn dos inimigos
    [SerializeField] private int maxEnemies = 5; // M�ximo de inimigos ativos ao mesmo tempo

    [SerializeField] private float spawnInterval = 5f; // Intervalo de tempo entre spawns
    private float spawnTimer = 0f; // Temporizador para controle do intervalo de spawn

    private void Start()
    {
        spawnTimer = spawnInterval; // Come�a com o timer completo para o primeiro spawn
    }

    private void Update()
    {
        // Conta o tempo para o pr�ximo spawn
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnEnemies(); // Realiza o spawn dos inimigos
            spawnTimer = spawnInterval; // Reseta o temporizador
        }
    }

    private void SpawnEnemies()
    {
        // Verifica se h� pontos de spawn e se h� prefabs de inimigos configurados
        if (spawnPoints.Length > 0 && enemyPrefabs.Length > 0)
        {
            int currentEnemies = CountCurrentEnemies();

            // Calcula quantos inimigos precisam ser spawnados para atingir o limite m�ximo
            int enemiesToSpawn = Mathf.Min(maxEnemies - currentEnemies, maxEnemies);

            // Itera para spawnar os inimigos
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                // Escolhe aleatoriamente um prefab de inimigo
                GameObject chosenEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

                // Escolhe aleatoriamente um ponto de spawn
                int randomIndex = Random.Range(0, spawnPoints.Length);
                Transform spawnPoint = spawnPoints[randomIndex];

                // Spawn do inimigo no ponto escolhido
                GameObject spawnedEnemy = Instantiate(chosenEnemyPrefab, spawnPoint.position, spawnPoint.rotation);
                // Aqui voc� pode configurar qualquer outra coisa do inimigo, como sa�de, comportamento, etc.
            }
        }
    }

    private int CountCurrentEnemies()
    {
        // Conta quantos inimigos est�o ativos no momento
        EnemyHealth[] enemies = FindObjectsOfType<EnemyHealth>();
        return enemies.Length;
    }
}