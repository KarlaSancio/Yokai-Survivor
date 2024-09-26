using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public EnemyScriptableObject enemyStats;

    // Stats atuais
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;

    public float despawnDistance = 18f;
    Transform player;

    void Awake()
    {
        currentMoveSpeed = enemyStats.MoveSpeed;
        currentHealth = enemyStats.MaxHealth;
        currentDamage = enemyStats.Damage;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform; // Encontra o jogador
    }

    void Update()
    {
        // Se a distancia entre o inimigo e o jogador for maior que a distancia de despawn, o inimigo é destruido
        if(Vector2.Distance(transform.position, player.position) >= despawnDistance)
        {
            RelocateEnemy(); // Move o inimgo de volta para o local de spawn
        }
    }

    public void GripDamage(float dmg)
    {
       currentHealth -= dmg;  

         if(currentHealth <= 0)
         {
              KillEnemy();
         }
    }

    void KillEnemy()
    {
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player") // Se o objeto colidido for o player
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.DamagePlayer(currentDamage); // Chama a função de dano do player
        }
    }

    private void OnDestroy()
    {
        // Se o inimigo for morto, diminui a quantidade de inimigos vivos
        EnemySpawn es = FindObjectOfType<EnemySpawn>();
        es.DecrementAliveEnemies();
    }

    public void RelocateEnemy()
    {
        EnemySpawn es = FindObjectOfType<EnemySpawn>();
        transform.position = player.position + es.relativeSpawnPoints[Random.Range(0, es.relativeSpawnPoints.Count)].position;
    }

}
