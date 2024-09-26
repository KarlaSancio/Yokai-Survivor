using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCont : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkRadius; // Variavel de controle para o raio de verificacao de terreno
    Vector3 noTerrarain; // Para onde o player vai se nao tiver terreno
    public LayerMask terrainMask; // Para o layer de terreno
    PlayerMovement pm;
    public GameObject currentChunk;

    [Header("Otimizacao")]
    public List<GameObject> spawnedChunks;
    public GameObject latestChunk; // Ultimo terreno spawnado
    public float maxDistance; // Distancia maxima para spawnar terreno (maior que largura e altura do tilemap)
    float opDistance; // Distancia entre o player e o ultimo terreno spawnado
    float optimizerCooldown; // Cooldown para otimizar
    public float optimizerCooldownTime; // Tempo de cooldown para otimizar

    // Start is called before the first frame update
    void Start()
    {
       pm = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckTerrain();
        Optimizer();
    }

    void CheckTerrain()
    {
        if (!currentChunk)
        {
            return;
        }

        //Checar a direcao q o player esta indo
        if (pm.moveDirection.x > 0 && pm.moveDirection.y == 0) // Direita
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Direita").position, checkRadius, terrainMask))
            {
                noTerrarain = currentChunk.transform.Find("Direita").position;
                SpawnChunk();
            }
        }
        else if (pm.moveDirection.x < 0 && pm.moveDirection.y == 0) // Esquerda
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Esquerda").position, checkRadius, terrainMask))
            {
                noTerrarain = currentChunk.transform.Find("Esquerda").position;
                SpawnChunk();
            }
        }
        else if (pm.moveDirection.x == 0 && pm.moveDirection.y > 0) // Cima
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Cima").position, checkRadius, terrainMask))
            {
                noTerrarain = currentChunk.transform.Find("Cima").position;
                SpawnChunk();
            }
        }
        else if (pm.moveDirection.x == 0 && pm.moveDirection.y < 0) // Baixo
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Baixo").position, checkRadius, terrainMask))
            {
                noTerrarain = currentChunk.transform.Find("Baixo").position;
                SpawnChunk();
            }
        }
        else if (pm.moveDirection.x > 0 && pm.moveDirection.y > 0) // Direita Cima
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Direita Cima").position, checkRadius, terrainMask))
            {
                noTerrarain = currentChunk.transform.Find("Direita Cima").position;
                SpawnChunk();
            }
        }
        else if (pm.moveDirection.x > 0 && pm.moveDirection.y < 0) // Direita Baixo
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Direita Baixo").position, checkRadius, terrainMask))
            {
                noTerrarain = currentChunk.transform.Find("Direita Baixo").position;
                SpawnChunk();
            }
        }
        else if (pm.moveDirection.x < 0 && pm.moveDirection.y > 0) // Esquerda Cima
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Esquerda Cima").position, checkRadius, terrainMask))
            {
                noTerrarain = currentChunk.transform.Find("Esquerda Cima").position;
                SpawnChunk();
            }
        }
        else if (pm.moveDirection.x < 0 && pm.moveDirection.y < 0) // Esquerda Baixo
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Esquerda Baixo").position, checkRadius, terrainMask))
            {
                noTerrarain = currentChunk.transform.Find("Esquerda Baixo").position;
                SpawnChunk();
            }
        }   
    }
    void SpawnChunk()
    {
        int index = Random.Range(0, terrainChunks.Count);
        latestChunk =  Instantiate(terrainChunks[index], noTerrarain, Quaternion.identity);
        spawnedChunks.Add(latestChunk);

    }
    void Optimizer()
    {
        optimizerCooldown -= Time.deltaTime;

        if(optimizerCooldown <= 0f)
        {
            optimizerCooldown = optimizerCooldownTime;
        }
        else
        {
            return;
        }

        foreach(GameObject chunk in spawnedChunks)
        {
            opDistance = Vector3.Distance(player.transform.position, chunk.transform.position);
            if(opDistance > maxDistance)
            {
               chunk.SetActive(false);
            }
            else
            {
                chunk.SetActive(true);
            }
        }
    }
}
