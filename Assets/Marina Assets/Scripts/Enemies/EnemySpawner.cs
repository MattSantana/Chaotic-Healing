using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs; // Array de prefabs de inimigos para escolher aleatoriamente
    [SerializeField] private Transform[] spawnPoints; // Pontos de spawn dos inimigos
    [SerializeField] private int maxEnemies = 5; // Máximo de inimigos ativos ao mesmo tempo

    [SerializeField] private float spawnInterval = 5f; // Intervalo de tempo entre spawns
    private float spawnTimer = 0f; // Temporizador para controle do intervalo de spawn

    private void Start()
    {
        spawnTimer = spawnInterval; // Começa com o timer completo para o primeiro spawn
    }

    private void Update()
    {
        // Conta o tempo para o próximo spawn
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnEnemies(); // Realiza o spawn dos inimigos
            spawnTimer = spawnInterval; // Reseta o temporizador
        }
    }

    private void SpawnEnemies()
    {
        // Verifica se há pontos de spawn e se há prefabs de inimigos configurados
        if (spawnPoints.Length > 0 && enemyPrefabs.Length > 0)
        {
            int currentEnemies = CountCurrentEnemies();

            // Calcula quantos inimigos precisam ser spawnados para atingir o limite máximo
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
                // Aqui você pode configurar qualquer outra coisa do inimigo, como saúde, comportamento, etc.
            }
        }
    }

    private int CountCurrentEnemies()
    {
        // Conta quantos inimigos estão ativos no momento
        EnemyHealth[] enemies = FindObjectsOfType<EnemyHealth>();
        return enemies.Length;
    }
}