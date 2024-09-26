using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{

    // Criacao de uma classe aninhada para armazenar os dados das "ondas" de inimigos

    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyClass> enemyClasses; // Lista de inimigos que serao spawnados nessa onda
        public int totalEnemies; // Total de inimigos que serao spawnados nessa onda
        public float spawnRate; // Taxa de spawn
        public int spawnCount; // Quantidade de inimigos que ja foram spawnados

    }

    [System.Serializable]
    public class EnemyClass
    {
        public string enemyName; // Nome do inimigo
        public GameObject enemyPrefab; // Prefab do inimigo que sera spawnado
        public int enemyCount; // Quantidade de inimigos que serao spawnados
        public int spawnCount; // Quantidade de inimigos que ja foram spawnados

    }

    public List<Wave> waves; // Lista de ondas de inimigos

    public int totalWavesCount; // Index da onda atual

    [Header("Spawn Settings")]
    float spawnTimer; // Timer de spawn que determina quando os inimigos serao spawnados
    public float waveInterval; // Intervalo entre as ondas de inimigos
    public int aliveEnemies; // Quantidade de inimigos vivos
    public int maxEnemiesOnscreen; // Quantidade maxima de inimigos que podem estar na tela
    public bool maxEnemiesHandled = false; // Uma flag para verificar se a quantidade maxima de inimigos ja foi atingida

    [Header("Spawn Points")]
    public List<Transform> relativeSpawnPoints; // Lista de pontos de spawn



    Transform player; // Referencia ao jogador

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerStats>().transform;
        ComputeWaveShare();
    }

    // Update is called once per frame
    void Update()
    {

        if(totalWavesCount < waves.Count && waves[totalWavesCount].spawnCount == 0) // se a onda terminou de spawnar todos os inimigos, chama a proxima onda
        {
            StartCoroutine(NextWave()); // Chama a proxima onda de inimigos
        }

        spawnTimer += Time.deltaTime;

        if(spawnTimer >= waves[totalWavesCount].spawnRate) // Verifica se o timer de spawn e maior ou igual a taxa de spawn
        {
            spawnTimer = 0f;
            SpawnEnemy(); // Chama a proxima onda de inimigos
        }
    }

    // Usando Coroutine para chamar a proxima onda de inimigos
    IEnumerator NextWave()
    {
        yield return new WaitForSeconds(waveInterval);
        if(totalWavesCount < waves.Count - 1)
        {
            totalWavesCount++;
            ComputeWaveShare();
        }
    }

    void ComputeWaveShare()
    {
        // Calcula a quantidade de inimigos que serao spawnados em cada onda
        int currentWaveShare = 0;
        foreach (var enemyClasses in waves[totalWavesCount].enemyClasses)
        {
            currentWaveShare += enemyClasses.enemyCount;
        }

        waves[totalWavesCount].totalEnemies = currentWaveShare;
        Debug.LogWarning(currentWaveShare);

    }

    /// <summary>
    /// Metodo para spawnar os inimigos
    /// </summary>

    void SpawnEnemy()
    {
        // Spawna os inimigos
        if(waves[totalWavesCount].spawnCount < waves[totalWavesCount].totalEnemies && !maxEnemiesHandled) 
        {
            // Spawna os inimigos de acordo com a quantidade de inimigos que serao spawnados
            foreach (var enemyClasses in waves[totalWavesCount].enemyClasses)
            {
                if(enemyClasses.spawnCount < enemyClasses.enemyCount) // Verifica se a quantidade de inimigos que ja foram spawnados e menor que a quantidade de inimigos que serao spawnados
                {
                    if(aliveEnemies >= maxEnemiesOnscreen) // Verifica se a quantidade de inimigos vivos e maior ou igual a quantidade maxima de inimigos que podem estar na tela
                    {
                        maxEnemiesHandled = true; // Seta a flag para true
                        return;
                    }

                    Instantiate(enemyClasses.enemyPrefab, player.position + relativeSpawnPoints[UnityEngine.Random.Range(0, relativeSpawnPoints.Count)].position, Quaternion.identity); // Spawna o inimigo em um ponto de spawn aleatorio

                    enemyClasses.spawnCount++; // Incrementa a quantidade de inimigos que ja foram spawnados
                    waves[totalWavesCount].spawnCount++; // Incrementa a quantidade de inimigos que ja foram spawnados
                    aliveEnemies++; // Incrementa a quantidade de inimigos vivos
                }
            }
        }

        if(aliveEnemies < maxEnemiesOnscreen) // reseta a flag para false
        {
            maxEnemiesHandled = false;
        }
    }

    public void DecrementAliveEnemies()
    {
        aliveEnemies--; // Decrementa a quantidade de inimigos vivos
    }
}
