using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    EnemyData enemy;
    Transform player;


    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<EnemyData>(); // Pega o script de dados do inimigo
        player = FindObjectOfType<PlayerMovement>().transform; // Encontra o player
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemy.currentMoveSpeed * Time.deltaTime); // Inimigo move em direcao ao player

        // // Calculate the direction to the player
        // Vector2 direction = (player.position - transform.position).normalized;

        // // Determine the angle to rotate towards the player
        // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // // Apply the rotation to face the player
        // transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); 
    }
}
